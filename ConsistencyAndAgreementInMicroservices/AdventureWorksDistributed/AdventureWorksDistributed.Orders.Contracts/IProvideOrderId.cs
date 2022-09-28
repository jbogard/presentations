namespace AdventureWorksDistributed.Orders.Contracts;

public interface IProvideOrderId
{
    public Guid OrderId { get; }
}