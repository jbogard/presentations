using NServiceBus;

namespace AdventureWorksDistributed.Orders.Contracts;

public class CancelOrderRequest : ICommand, IProvideOrderId
{
    public Guid OrderId { get; set; }
}