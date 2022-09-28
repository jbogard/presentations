using AdventureWorksDistributed.Core.Infrastructure;
using AdventureWorksDistributed.Inventory.Contracts;
using Microsoft.Azure.Cosmos;
using NServiceBus;

namespace AdventureWorksDistributed.Inventory.Endpoint;

public class StockReturnRequestedHandler : IHandleMessages<StockReturnRequest>
{
    private readonly IDocumentDbRepository<Stock> _repository;

    public StockReturnRequestedHandler(IDocumentDbRepository<Stock> repository)
        => _repository = repository;

    public async Task Handle(StockReturnRequest message, IMessageHandlerContext context)
    {
        var stock = (await _repository
                .GetItemsAsync(s => s.ProductId == message.ProductId))
            .FirstOrDefault() ?? new Stock
            {
                Id = Guid.NewGuid(),
                ProductId = message.ProductId,
                QuantityAvailable = 100
            };

        stock.QuantityAvailable += message.AmountToReturn;

        await _repository.UpdateItemAsync(stock);
    }
}