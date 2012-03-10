using System;
using NServiceBus;

namespace FileProducerAfterMessages
{
    public class ExportFile : ICommand
    {
        public Guid BatchId { get; set; } 
    }
}