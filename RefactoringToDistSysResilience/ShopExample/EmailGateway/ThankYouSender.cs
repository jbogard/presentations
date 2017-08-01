using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using OrderProcessing.Messages;

namespace EmailGateway
{
    public class ThankYouSender : IHandleMessages<OrderAcceptedEvent>
    {
        private static ILog _log = LogManager.GetLogger<ThankYouSender>();

        public Task Handle(OrderAcceptedEvent message, IMessageHandlerContext context)
        {
            _log.InfoFormat("Sending email for order ID  {0}", message.OrderId);

            return Task.FromResult(0);
        }
    }
}