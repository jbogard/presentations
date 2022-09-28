using AdventureWorksDistributed.Core.Infrastructure;
using AdventureWorksDistributed.Orders.Api.Models;
using AdventureWorksDistributed.Orders.Contracts;
using Microsoft.Azure.Cosmos;
using NServiceBus;

namespace AdventureWorksDistributed.Orders.Api.Handlers;

public class CancelOrderRequestHandler : IHandleMessages<CancelOrderRequest>
{
    private readonly IDocumentDbRepository<OrderRequest> _repository;

    public CancelOrderRequestHandler(IDocumentDbRepository<OrderRequest> repository)
    {
        _repository = repository;
    }

    public async Task Handle(CancelOrderRequest message, IMessageHandlerContext context)
    {
        var session = context.SynchronizedStorageSession.CosmosPersistenceSession();

        var order = await _repository.GetItemAsync(message.OrderId);

        if (order.Status == Status.Rejected)
            return;

        order.Status = Status.Cancelled;

        var requestOptions = new TransactionalBatchItemRequestOptions
        {
            EnableContentResponseOnWrite = false,
        };

        session.Batch.UpsertItem(order, requestOptions);
    }
}