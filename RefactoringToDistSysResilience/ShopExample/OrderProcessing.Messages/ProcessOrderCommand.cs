using System;
using NServiceBus;

namespace OrderProcessing.Messages
{
    public class ProcessOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
    }
}