using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence;
using PaymentGateway.Messages;

namespace OrderProcessing
{
    [DesignerCategory("Code")]
    public class ProgramService : ServiceBase
    {
        private IEndpointInstance _endpoint;

        static readonly ILog Logger;

        static ProgramService()
        {
            //TODO: optionally choose a custom logging library
            //https://docs.particular.net/nservicebus/logging/#custom-logging
            // LogManager.Use<TheLoggingFactory>();
            Logger = LogManager.GetLogger<ProgramService>();
        }

        public static void Main()
        {
            using (var service = new ProgramService())
            {
                // to run interactive from a console or as a windows service
                if (Environment.UserInteractive)
                {
                    Console.Title = "OrderProcessing";
                    Console.CancelKeyPress += (sender, e) =>
                    {
                        service.OnStop();
                    };
                    service.OnStart(null);
                    Console.WriteLine("\r\nPress enter key to stop program\r\n");
                    Console.Read();
                    service.OnStop();
                    return;
                }
                Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            AsyncOnStart().GetAwaiter().GetResult();
        }

        async Task AsyncOnStart()
        {
            try
            {
                var endpointConfiguration = new EndpointConfiguration("ShopExample.OrderProcessing");
                endpointConfiguration.UseSerialization<JsonSerializer>();
                endpointConfiguration.SendFailedMessagesTo("error");
                endpointConfiguration.AuditProcessedMessagesTo("audit");
                endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

                var transport = endpointConfiguration.UseTransport<RabbitMQTransport>().ConnectionString("host=localhost");
                endpointConfiguration.UsePersistence<NHibernatePersistence>().ConnectionString("Server=(localdb)\\mssqllocaldb;Database=ShopExample;Trusted_Connection=True;MultipleActiveResultSets=true");
                endpointConfiguration.EnableInstallers();

                var routing = transport.Routing();
                routing.RouteToEndpoint(
                    assembly: typeof(ProcessPaymentCommand).Assembly,
                    destination: "ShopExample.PaymentGateway");

                _endpoint = await Endpoint.Start(endpointConfiguration)
                    .ConfigureAwait(false);
                PerformStartupOperations();
            }
            catch (Exception exception)
            {
                Exit("Failed to start", exception);
            }
        }

        void Exit(string failedToStart, Exception exception)
        {
            Logger.Fatal(failedToStart, exception);
            //TODO: When using an external logging framework it is important to flush any pending entries prior to calling FailFast
            // https://docs.particular.net/nservicebus/hosting/critical-errors#when-to-override-the-default-critical-error-action
            Environment.FailFast(failedToStart, exception);
        }

        void PerformStartupOperations()
        {
            //TODO: perform any startup operations
        }

        Task OnCriticalError(ICriticalErrorContext context)
        {
            //TODO: Decide if shutting down the process is the best response to a critical error
            // https://docs.particular.net/nservicebus/hosting/critical-errors
            var fatalMessage = $"The following critical error was encountered:\n{context.Error}\nProcess is shutting down.";
            Exit(fatalMessage, context.Exception);
            return Task.FromResult(0);
        }

        protected override void OnStop()
        {
            _endpoint?.Stop().GetAwaiter().GetResult();
        }
    }
}