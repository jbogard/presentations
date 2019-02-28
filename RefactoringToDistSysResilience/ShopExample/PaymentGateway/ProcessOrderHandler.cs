using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using PaymentGateway.Messages;

namespace PaymentGateway
{
    public static class StripeService
    {
        private static bool _success;

        public static Task<bool> ProcessPayment(ProcessPaymentCommand message)
        {
            _success = !_success;
            return Task.FromResult(_success);
        }
    }

    public class ProcessOrderHandler 
        : IHandleMessages<ProcessPaymentCommand>
    {
        public async Task Handle(ProcessPaymentCommand message, 
            IMessageHandlerContext context)
        {
            var result = await StripeService.ProcessPayment(message);

            var reply = new ProcessPaymentResult
            {
                Success = result
            };

            _log.InfoFormat("Payment for order {0} success: {1}", 
                message.OrderId, reply.Success);

            await context.Reply(result);
        }

        private static ILog _log = LogManager.GetLogger<ProcessOrderHandler>();

    }
}