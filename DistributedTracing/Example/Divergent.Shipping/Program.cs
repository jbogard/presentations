using Divergent.ITOps.Messages.Commands;
using ITOps.EndpointConfig;
using NServiceBus;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

const string EndpointName = "Divergent.Shipping";

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
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

        endpoint.Configure(routing =>
        {
            routing.RouteToEndpoint(typeof(ShipWithFedexCommand), "Divergent.ITOps");
        });

        return endpoint;
    }).Build();

var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();

Console.Title = hostEnvironment.ApplicationName;

host.Run();