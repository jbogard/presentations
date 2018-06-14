using AdventureWorksCosmos.UI.Infrastructure;

namespace AdventureWorksCosmos.UI.Models.Inventory
{
    public class Stock : AggregateBase
    {
        public int QuantityAvailable { get; set; }

        public int ProductId { get; set; }
    }
}