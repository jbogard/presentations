using NServiceBus;

namespace AdventureWorksDistributed.Orders.Contracts;

public class OrderRejected : IEvent, IProvideOrderId
{
    public Guid OrderId { get; set; }
}