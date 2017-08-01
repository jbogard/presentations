using NServiceBus;

namespace OrderProcessing.Messages
{
    public class OrderAcceptedEvent : IEvent
    {
        public int OrderId { get; set; }
    }
}