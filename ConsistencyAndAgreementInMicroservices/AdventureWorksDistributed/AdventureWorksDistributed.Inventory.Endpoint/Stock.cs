using AdventureWorksDistributed.Core;

namespace AdventureWorksDistributed.Inventory.Endpoint;

public class Stock : DocumentBase
{
    public int QuantityAvailable { get; set; }

    public int ProductId { get; set; }
}