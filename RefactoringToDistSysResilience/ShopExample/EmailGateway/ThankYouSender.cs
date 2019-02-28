using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using OrderProcessing.Messages;

namespace EmailGateway
{
    public static class SendGridService
    {
        public static Task SendThankYouEmail(OrderAcceptedEvent message)
        {
            return Task.CompletedTask;
        }
    }

    public class ThankYouSender 
        : IHandleMessages<OrderAcceptedEvent>
    {
        public Task Handle(OrderAcceptedEvent message, 
            IMessageHandlerContext context)
        {
            _log.InfoFormat("Sending email for order ID {0}", message.OrderId);

            return SendGridService.SendThankYouEmail(message);
        }

        private static ILog _log = LogManager.GetLogger<ThankYouSender>();
    }
}