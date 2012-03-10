using System;
using NServiceBus;

namespace OrderProcessorMessages
{
    public class PlaceOrder : ICommand
    {
        public Guid ClientOrderId { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }
    }
}