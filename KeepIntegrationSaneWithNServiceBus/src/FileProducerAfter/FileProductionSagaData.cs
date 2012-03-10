using System;
using NServiceBus.Saga;

namespace FileProducerAfter
{
    public class FileProductionSagaData : IContainSagaData
    {
        public virtual Guid Id { get; set; }
        public virtual string Originator { get; set; }
        public virtual string OriginalMessageId { get; set; }

        public virtual Guid BatchId { get; set; }
    }
}