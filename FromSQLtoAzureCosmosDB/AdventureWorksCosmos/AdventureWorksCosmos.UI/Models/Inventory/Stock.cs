using AdventureWorksCosmos.UI.Infrastructure;
using AdventureWorksCosmos.UI.Models.Orders;

namespace AdventureWorksCosmos.UI.Models.Inventory
{
    public class Stock : DocumentBase
    {
        public int QuantityAvailable { get; set; }

        public int ProductId { get; set; }

        public void Handle(ItemPurchased message)
        {
            Process(message, e =>
            {
                QuantityAvailable -= e.Quantity;
            });
        }
    }
}