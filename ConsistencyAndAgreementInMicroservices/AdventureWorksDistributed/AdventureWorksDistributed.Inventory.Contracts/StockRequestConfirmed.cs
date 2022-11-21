using AdventureWorksDistributed.PartitionKeys;
using NServiceBus;

namespace AdventureWorksDistributed.Inventory.Contracts;

public class StockRequestConfirmed : IMessage, IProvideOrderId
{
    public Guid OrderId { get; set; }

    public int ProductId { get; set; }
}