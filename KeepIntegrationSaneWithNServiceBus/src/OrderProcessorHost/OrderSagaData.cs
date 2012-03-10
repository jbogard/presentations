using System;
using NServiceBus.Saga;

namespace OrderProcessorHost
{
    public class OrderSagaData : IContainSagaData
    {
        public virtual Guid Id { get; set; }
        public virtual string Originator { get; set; }
        public virtual string OriginalMessageId { get; set; }

        public virtual Guid ClientOrderId { get; set; }

        public virtual int OrderId { get; set; }
    }
}