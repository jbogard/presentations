using Divergent.Finance.Data.Context;
using Divergent.Finance.Data.Migrations;
using Divergent.Finance.PaymentClient;
using ITOps.EndpointConfig;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

const string EndpointName = "Divergent.Finance";

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddSingleton<ReliablePaymentClient>();
        services.AddDbContext<FinanceContext>(options =>
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
            .AddHttpClientInstrumentation()
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
        var context = services.GetRequiredService<FinanceContext>();
        DatabaseInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}
