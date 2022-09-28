using AdventureWorksDistributed.Fulfillment.Contracts;
using AdventureWorksDistributed.Inventory.Contracts;
using AdventureWorksDistributed.Orders.Contracts;
using NServiceBus;

namespace AdventureWorksDistributed.Fulfillment.Endpoint;

public class OrderFulfillmentSaga : Saga<OrderFulfillment>,
    IAmStartedByMessages<OrderCreated>,
    IHandleMessages<OrderApproved>,
    IHandleMessages<StockRequestConfirmed>,
    IHandleMessages<StockRequestDenied>,
    IHandleMessages<OrderRejected>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderFulfillment> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<OrderCreated>(message => message.OrderId)
            .ToMessage<OrderApproved>(message => message.OrderId)
            .ToMessage<OrderRejected>(message => message.OrderId)
            .ToMessage<StockRequestConfirmed>(message => message.OrderId)
            .ToMessage<StockRequestDenied>(message => message.OrderId);
    }

    public async Task Handle(OrderCreated message, IMessageHandlerContext context)
    {
        if (Data.IsCancelled)
            return;

        Data.LineItems = message.LineItems
            .Select(li => new OrderFulfillment.LineItem
            {
                ProductId = li.ProductId,
                AmountRequested = li.Quantity
            })
            .ToList();

        foreach (var lineItem in Data.LineItems)
        {
            await context.Send(new StockRequest
            {
                ProductId = lineItem.ProductId,
                AmountRequested = lineItem.AmountRequested,
                OrderId = message.OrderId
            });
        }
    }

    public async Task Handle(OrderApproved message, IMessageHandlerContext context)
    {
        Data.OrderApproved = true;

        if (Data.IsCancelled)
        {
            await ProcessCancellation(context);
        }
        else
        {
            await CheckForSuccess(context);
        }
    }

    public async Task Handle(StockRequestConfirmed message, IMessageHandlerContext context)
    {
        var lineItem = Data.LineItems.Single(li => li.ProductId == message.ProductId);
        lineItem.StockConfirmed = true;

        if (Data.IsCancelled)
        {
            await ReturnStock(lineItem, context);
        }
        else
        {
            await CheckForSuccess(context);
        }
    }

    public async Task Handle(StockRequestDenied message, IMessageHandlerContext context)
    {
        await Cancel(context);
    }

    public async Task Handle(OrderRejected message, IMessageHandlerContext context)
    {
        Data.OrderRejected = true;

        await Cancel(context);
    }

    private async Task CheckForSuccess(IMessageHandlerContext context)
    {
        if (Data.IsCancelled)
            return;

        if (Data.LineItems.All(li => li.StockConfirmed) && Data.OrderApproved)
        {
            await context.Publish(new OrderFulfillmentSuccessful
            {
                OrderId = Data.OrderId
            });
        }
    }

    private async Task Cancel(IMessageHandlerContext context)
    {
        Data.IsCancelled = true;

        await ProcessCancellation(context);
    }

    private async Task ProcessCancellation(IMessageHandlerContext context)
    {
        if (!Data.CancelOrderRequested && !Data.OrderRejected)
        {
            Data.CancelOrderRequested = true;
            await context.Send(new CancelOrderRequest
            {
                OrderId = Data.OrderId
            });
        }

        foreach (var lineItem in Data.LineItems.Where(li => li.StockConfirmed))
        {
            await ReturnStock(lineItem, context);
        }
    }

    private async Task ReturnStock(OrderFulfillment.LineItem lineItem,
        IMessageHandlerContext context)
    {
        if (lineItem.StockReturnRequested)
            return;

        lineItem.StockReturnRequested = true;

        await context.Send(new StockReturnRequest
        {
            ProductId = lineItem.ProductId,
            AmountToReturn = lineItem.AmountRequested,
            OrderId = Data.OrderId
        });
    }
}