using AdventureWorksDistributed.Orders.Contracts;
using NServiceBus;

namespace AdventureWorksDistributed.Inventory.Contracts;

public class StockRequest : ICommand, IProvideProductId, IProvideOrderId
{
    public Guid OrderId { get; set; }
    public int ProductId { get; set; }
    public int AmountRequested { get; set; }
}