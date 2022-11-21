using AdventureWorksDistributed.PartitionKeys;
using NServiceBus;

namespace AdventureWorksDistributed.Fulfillment.Contracts;

public class OrderFulfillmentSuccessful : IEvent, IProvideOrderId
{
    public Guid OrderId { get; set; }

}