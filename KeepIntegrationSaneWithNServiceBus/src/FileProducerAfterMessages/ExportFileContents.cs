using System;
using NServiceBus;

namespace FileProducerAfterMessages
{
    public class ExportFileContents : ICommand
    {
        public Guid BatchId { get; set; }
    }
}