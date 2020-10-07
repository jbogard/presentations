using System;
using System.Diagnostics;
using Divergent.ITOps.Messages.Commands;
using ITOps.EndpointConfig;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Extensions.Diagnostics.OpenTelemetry;
using OpenTelemetry.Trace;

namespace Divergent.Shipping
{
    public class Program
    {
        public static string EndpointName => "Divergent.Shipping";

        public static void Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddOpenTelemetryTracing(config => config
                        .AddZipkinExporter(o =>
                        {
                            o.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
                            o.ServiceName = EndpointName;
                        })
                        .AddJaegerExporter(c =>
                        {
                            c.AgentHost = "localhost";
                            c.AgentPort = 6831;
                            c.ServiceName = EndpointName;
                        })
                        .AddNServiceBusInstrumentation(opt => opt.CaptureMessageBody = true)
                        .AddSqlClientInstrumentation(opt => opt.SetTextCommandContent = true)
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
}
