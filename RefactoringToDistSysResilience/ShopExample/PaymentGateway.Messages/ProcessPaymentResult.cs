using NServiceBus;

namespace PaymentGateway.Messages
{
    public class ProcessPaymentResult : IMessage
    {
        public bool Success { get; set; }
    }
}