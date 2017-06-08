using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Polyglot.Orders.Processor.Messages
{
    public interface IOrderApprovedMessage : IEvent
    {
        Guid OrderId { get; set; }
        List<LineItem> LineItems { get; set; }
    }

    public class LineItem
    {
        public int ProductId { get; set; }
    }
}
