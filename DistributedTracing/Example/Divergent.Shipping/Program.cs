using System;
using Divergent.ITOps.Messages.Commands;
using ITOps.EndpointConfig;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Divergent.Shipping;

public class Program
{
    public static string EndpointName => "Divergent.Shipping";

    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
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
                    .AddNServiceBusInstrumentation()
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
            });
}