using NServiceBus;

namespace AdventureWorksDistributed.Orders.Contracts;

public class OrderApproved : IEvent, IProvideOrderId
{
    public Guid OrderId { get; set; }
}