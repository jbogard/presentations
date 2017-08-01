using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using PaymentGateway.Messages;

namespace PaymentGateway
{
    public class ProcessOrderHandler : IHandleMessages<ProcessPaymentCommand>
    {
        private static bool _success;
        private static ILog _log = LogManager.GetLogger<ProcessOrderHandler>();

        public Task Handle(ProcessPaymentCommand message, 
            IMessageHandlerContext context)
        {
            var result = new ProcessPaymentResult
            {
                Success = _success
            };
            _success = !_success;

            _log.InfoFormat("Payment for order {0} success: {1}", message.OrderId, result.Success);

            return context.Reply(result);
        }
    }
}