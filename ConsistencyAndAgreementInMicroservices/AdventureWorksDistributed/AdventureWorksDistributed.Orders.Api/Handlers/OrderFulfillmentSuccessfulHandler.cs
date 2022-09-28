using AdventureWorksDistributed.Core.Infrastructure;
using AdventureWorksDistributed.Fulfillment.Contracts;
using AdventureWorksDistributed.Orders.Api.Models;
using AdventureWorksDistributed.Orders.Contracts;
using Microsoft.Azure.Cosmos;
using NServiceBus;

namespace AdventureWorksDistributed.Orders.Api.Handlers;

public class OrderFulfillmentSuccessfulHandler : IHandleMessages<OrderFulfillmentSuccessful>
{
    private readonly IDocumentDbRepository<OrderRequest> _repository;

    public OrderFulfillmentSuccessfulHandler(IDocumentDbRepository<OrderRequest> repository)
        => _repository = repository;

    public async Task Handle(OrderFulfillmentSuccessful message, IMessageHandlerContext context)
    {
        var session = context.SynchronizedStorageSession.CosmosPersistenceSession();

        var order = await _repository.GetItemAsync(message.OrderId);

        if (order.Status is Status.Rejected or Status.Cancelled)
            return;

        order.Status = Status.Completed;

        var requestOptions = new TransactionalBatchItemRequestOptions
        {
            EnableContentResponseOnWrite = false,
        };

        session.Batch.UpsertItem(order, requestOptions);
    }
}