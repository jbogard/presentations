using System;
using System.Diagnostics;
using System.IO;
using Divergent.Sales.Messages.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Divergent.Sales.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseNServiceBus(context =>
                {
                    var config = new EndpointConfiguration("Sales.API");

                    config.SendOnly();

                    var transport = config.UseTransport<LearningTransport>();

                    var routing = transport.Routing();

                    routing.RouteToEndpoint(typeof(SubmitOrderCommand), "Divergent.Sales");

                    config.UseSerialization<NewtonsoftSerializer>();
                    config.UsePersistence<LearningPersistence>();

                    config.SendFailedMessagesTo("error");

                    config.Conventions()
                        .DefiningCommandsAs(t => t.Namespace != null && t.Namespace == "Divergent.Messages" || t.Name.EndsWith("Command"))
                        .DefiningEventsAs(t => t.Namespace != null && t.Namespace == "Divergent.Messages" || t.Name.EndsWith("Event"));

                    return config;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
