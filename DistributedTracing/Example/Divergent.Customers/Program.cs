using Divergent.Customers.Data.Context;
using Divergent.Customers.Data.Migrations;
using ITOps.EndpointConfig;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

const string EndpointName = "Divergent.Customers";

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddDbContext<CustomersContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        services.AddOpenTelemetryTracing(config => config
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(EndpointName))
            .AddZipkinExporter(o =>
            {
                o.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
            })
            .AddJaegerExporter(c =>
            {
                c.AgentHost = "localhost";
                c.AgentPort = 6831;
            })
            .AddSource("NServiceBus.Core")
            .AddSqlClientInstrumentation(opt => opt.SetDbStatementForText = true)
        );
    })
    .UseNServiceBus(context =>
    {
        var endpoint = new EndpointConfiguration(EndpointName);
        endpoint.Configure();

        return endpoint;
    }).Build();

CreateDbIfNotExists(host);

var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();

Console.Title = hostEnvironment.ApplicationName;

host.Run();

static void CreateDbIfNotExists(IHost host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<CustomersContext>();
        DatabaseInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}
