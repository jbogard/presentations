using NServiceBus;

namespace AdventureWorksDistributed.Fulfillment.Contracts;

public class OrderFulfillmentSuccessful : IEvent
{
    public Guid OrderId { get; set; }

}