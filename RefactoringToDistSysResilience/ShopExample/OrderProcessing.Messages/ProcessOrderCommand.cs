using NServiceBus;

namespace OrderProcessing.Messages
{
    public class ProcessOrderCommand : ICommand
    {
        public int OrderId { get; set; }
    }
}