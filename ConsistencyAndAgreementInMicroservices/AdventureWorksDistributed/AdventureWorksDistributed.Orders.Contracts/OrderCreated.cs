using AdventureWorksDistributed.PartitionKeys;
using NServiceBus;

namespace AdventureWorksDistributed.Orders.Contracts;

public class OrderCreated : IEvent, IProvideOrderId
{
    public Guid OrderId { get; set; }

    public List<LineItem> LineItems { get; set; }

    public class LineItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}