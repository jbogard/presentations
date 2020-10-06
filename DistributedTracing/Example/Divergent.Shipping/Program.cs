using System.Diagnostics;
using Divergent.ITOps.Messages.Commands;
using ITOps.EndpointConfig;
using Microsoft.Extensions.Hosting;
using NServiceBus;

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
