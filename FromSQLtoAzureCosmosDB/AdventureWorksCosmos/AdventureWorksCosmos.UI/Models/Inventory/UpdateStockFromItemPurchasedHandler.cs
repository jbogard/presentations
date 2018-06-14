using System;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorksCosmos.UI.Infrastructure;
using AdventureWorksCosmos.UI.Models.Orders;

namespace AdventureWorksCosmos.UI.Models.Inventory
{
    public class UpdateStockFromItemPurchasedHandler : IDomainEventHandler<ItemPurchased>
    {
        private readonly IDocumentDBRepository<Stock> _repository;

        public UpdateStockFromItemPurchasedHandler(IDocumentDBRepository<Stock> repository) 
            => _repository = repository;

        public async Task Handle(ItemPurchased domainEvent)
        {
            var stock = (await _repository
                .GetItemsAsync(s => s.ProductId == domainEvent.ProductId))
                .FirstOrDefault();

            if (stock == null)
            {
                stock = new Stock
                {
                    Id = Guid.NewGuid(),
                    ProductId = domainEvent.ProductId,
                    QuantityAvailable = 100
                };

                await _repository.CreateItemAsync(stock);
            }

            stock.Handle(domainEvent);

            await _repository.UpdateItemAsync(stock);
        }
    }
}