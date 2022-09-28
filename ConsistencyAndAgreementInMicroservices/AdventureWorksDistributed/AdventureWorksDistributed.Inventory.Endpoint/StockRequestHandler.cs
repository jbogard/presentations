using AdventureWorksDistributed.Core.Infrastructure;
using AdventureWorksDistributed.Inventory.Contracts;
using Microsoft.Azure.Cosmos;
using NServiceBus;

namespace AdventureWorksDistributed.Inventory.Endpoint;

public class StockRequestHandler : IHandleMessages<StockRequest>
{
    private readonly IDocumentDbRepository<Stock> _repository;

    public StockRequestHandler(IDocumentDbRepository<Stock> repository)
        => _repository = repository;

    public async Task Handle(StockRequest message, IMessageHandlerContext context)
    {
        var stock = (await _repository
                .GetItemsAsync(s => s.ProductId == message.ProductId))
            .FirstOrDefault();

        if (stock == null)
        {
            stock = new Stock
            {
                Id = Guid.NewGuid(),
                ProductId = message.ProductId,
                QuantityAvailable = 100
            };

            await _repository.CreateItemAsync(stock);
        }

        if (stock.QuantityAvailable >= message.AmountRequested)
        {
            stock.QuantityAvailable -= message.AmountRequested;
            await context.Reply(new StockRequestConfirmed
            {
                ProductId = stock.ProductId,
                OrderId = message.OrderId
            });
        }
        else
        {
            await context.Reply(new StockRequestDenied
            {
                ProductId = stock.ProductId,
                OrderId = message.OrderId
            });
        }

        await _repository.UpdateItemAsync(stock);
    }
}