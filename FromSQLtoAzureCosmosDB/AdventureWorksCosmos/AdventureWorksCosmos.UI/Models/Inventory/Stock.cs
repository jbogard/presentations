using AdventureWorksCosmos.UI.Infrastructure;
using AdventureWorksCosmos.UI.Models.Orders;

namespace AdventureWorksCosmos.UI.Models.Inventory
{
    public class Stock : AggregateBase
    {
        public int QuantityAvailable { get; set; }

        public int ProductId { get; set; }

        public void Handle(ItemPurchased domainEvent)
        {
            Process(domainEvent, e =>
            {
                QuantityAvailable -= e.Quantity;
            });
        }
    }
}