using AdventureWorksDistributed.Orders.Contracts;
using NServiceBus;

namespace AdventureWorksDistributed.Inventory.Contracts;

public class StockReturnRequest : ICommand, IProvideProductId, IProvideOrderId
{
    public Guid OrderId { get; set; }
    public int ProductId { get; set; }
    public int AmountToReturn { get; set; }
}