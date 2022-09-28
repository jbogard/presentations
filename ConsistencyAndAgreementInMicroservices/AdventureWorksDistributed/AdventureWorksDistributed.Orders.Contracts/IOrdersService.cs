using AdventureWorksDistributed.Cart.Contracts;

namespace AdventureWorksDistributed.Orders.Contracts;

public interface IOrdersService
{
    Task<OrderRequestSummary> Get(Guid id);
    Task<Guid> Post(ShoppingCart cart);
    Task Approve(Guid id);
    Task Reject(Guid id);
}