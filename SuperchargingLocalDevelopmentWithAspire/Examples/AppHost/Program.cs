using System.Diagnostics;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddContainer("zipkin", "openzipkin/zipkin")
    .WithHttpEndpoint(9411, 9411);

builder.AddContainer("jaeger", "jaegertracing/all-in-one")
    .WithHttpEndpoint(16686, targetPort: 16686, name: "jaegerPortal")
    .WithHttpEndpoint(5317, targetPort: 4317, name: "jaegerEndpoint");

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
    .AddDatabase("sqldata")
    .WithCommand(
        name: $"migrate-sql-db",
        displayName: "Migrate",
        executeCommand: async cmd =>
        {
            var process = new Process();
            process.StartInfo.WorkingDirectory = $"../WebApplication";
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "ef database update";
            process.StartInfo.UseShellExecute = true;

            process.Start();

            await process.WaitForExitAsync(cmd.CancellationToken);

            return CommandResults.Success();
        },
        commandOptions: new CommandOptions { IconName = "ArrowSquareUpRight" }
    );;

var externalApi = builder
    .AddProject<ExternalApi>("externalapi")
    .WithUrlForEndpoint("http", url =>
    {
        url.DisplayText = "Scalar (HTTP)";
        url.Url = "/scalar";
    });;

var web = builder
    .AddProject<WebApplication>("web")
    .WithReference(externalApi)
    .WithReference(broker)
    .WithReference(sql)
    .WaitFor(broker)
    .WaitFor(sql);

var childWorker = builder
    .AddProject<ChildWorkerService>("childworker")
    .WithReference(broker)
    .WithReference(mongo)
    .WaitFor(broker);

var worker = builder
    .AddProject<WorkerService>("worker")
    .WithReference(broker)
    .WithReference(childWorker)
    .WithReference(web)
    .WaitFor(broker);

var application = builder.Build();

application.Run();

public partial class Program { }
