using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddContainer("zipkin", "openzipkin/zipkin")
    .WithEndpoint(9411, 9411);

builder.AddContainer("jaeger", "jaegertracing/all-in-one")
    .WithHttpEndpoint(16686, targetPort: 16686, name: "jaegerPortal")
    .WithHttpEndpoint(5317, targetPort: 4317, name: "jaegerEndpoint");

#region Enable OTel Collector with forwarding

// builder.AddOpenTelemetryCollector("collector", "config.yaml")
//     .WithAppForwarding();

#endregion

var rmqPassword = builder.AddParameter("messaging-password");
var dbPassword = builder.AddParameter("db-password");

var broker = builder.AddRabbitMQ(name: "broker", password: rmqPassword, port: 5672)
    .WithDataVolume()
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEndpoint("management", e => e.Port = 15672);

var mongo = builder.AddMongoDB("mongo")
    .WithLifetime(ContainerLifetime.Persistent);

var sql = builder.AddSqlServer("sql", password: dbPassword)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("sqldata");

//ConfigureParticularServicePlatform(builder, broker);

var web = builder
    .AddProject<Projects.WebApplication>("web")
    .WithReference(broker)
    .WithReference(sql)
    .WaitFor(broker)
    .WaitFor(sql);

var childWorker = builder
    .AddProject<Projects.ChildWorkerService>("childworker")
    .WithReference(broker)
    .WithReference(mongo)
    .WaitFor(broker);

var worker = builder
    .AddProject<Projects.WorkerService>("worker")
    .WithReference(broker)
    .WithReference(childWorker)
    .WithReference(web)
    .WaitFor(broker);

var application = builder.Build();

var logger = application.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation(builder.Configuration["DOTNET_DASHBOARD_OTLP_ENDPOINT_URL"]);
logger.LogInformation(builder.Configuration["AppHost:OtlpApiKey"]);
logger.LogInformation(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

application.Run();

static void ConfigureParticularServicePlatform(IDistributedApplicationBuilder builder,
    IResourceBuilder<RabbitMQServerResource> rabbitMqResource)
{
    var license = File.ReadAllText(
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "ParticularSoftware", 
            "license.xml"));
    
    builder
        .AddContainer("servicecontroldb", "particular/servicecontrol-ravendb", "latest")
        .WithBindMount("AppHost-servicecontroldb-data", "/opt/RavenDB/Server/RavenData")
        .WithEndpoint(8080, 8080);
    
    builder
        .AddContainer("servicecontrol", "particular/servicecontrol")
        .WithEnvironment("TransportType", "RabbitMQ.QuorumConventionalRouting")
        .WithEnvironment("ConnectionString", "host=host.docker.internal")
        .WithEnvironment("RavenDB_ConnectionString", "http://host.docker.internal:8080")
        .WithEnvironment("RemoteInstances", "[{\"api_uri\":\"http://host.docker.internal:44444/api\"}]")
        .WithEnvironment("PARTICULARSOFTWARE_LICENSE", license)
        .WithArgs("--setup-and-run")
        .WithContainerRuntimeArgs("-p", "33333:33333")
        .WaitFor(rabbitMqResource);

    builder
        .AddContainer("servicecontrolaudit", "particular/servicecontrol-audit")
        .WithEnvironment("TransportType", "RabbitMQ.QuorumConventionalRouting")
        .WithEnvironment("ConnectionString", "host=host.docker.internal")
        .WithEnvironment("RavenDB_ConnectionString", "http://host.docker.internal:8080")
        .WithEnvironment("PARTICULARSOFTWARE_LICENSE", license)
        .WithArgs("--setup-and-run")
        .WithEndpoint(44444, 44444)
        .WaitFor(rabbitMqResource);

    builder
        .AddContainer("servicecontrolmonitoring", "particular/servicecontrol-monitoring")
        .WithEnvironment("TransportType", "RabbitMQ.QuorumConventionalRouting")
        .WithEnvironment("ConnectionString", "host=host.docker.internal")
        .WithEnvironment("PARTICULARSOFTWARE_LICENSE", license)
        .WithArgs("--setup-and-run")
        .WithEndpoint(33633, 33633)
        .WaitFor(rabbitMqResource);

    builder
        .AddContainer("servicepulse", "particular/servicepulse")
        .WithEnvironment("SERVICECONTROL_URL", "http://host.docker.internal:33333")
        .WithEnvironment("MONITORING_URL", "http://host.docker.internal:33633")
        .WithEnvironment("PARTICULARSOFTWARE_LICENSE", license)
        .WithEndpoint(9090, 9090)
        .WaitFor(rabbitMqResource);
}

public partial class Program { }
