using NServiceBus;

namespace PaymentGateway.Messages
{
    public class ProcessPaymentCommand : ICommand
    {
        public int OrderId { get; set; }
    }
}