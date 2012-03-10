using System;
using NServiceBus;

namespace OrderProcessorMessages
{
    public class ShipOrder : ICommand
    {
        public Guid ClientOrderId { get; set; }
    }
}