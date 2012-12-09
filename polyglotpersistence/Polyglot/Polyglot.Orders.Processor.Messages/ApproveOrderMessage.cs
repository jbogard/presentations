using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Polyglot.Orders.Processor.Messages
{
    public class ApproveOrderMessage : ICommand
    {
        public Guid OrderId { get; set; }
    }
}
