using AdventureWorksDistributed.Cart.Contracts;
using AdventureWorksDistributed.Core;
using AdventureWorksDistributed.Orders.Contracts;

namespace AdventureWorksDistributed.Orders.Api.Models;

public class OrderRequest : DocumentBase
{
    public OrderRequest() { }

    public OrderRequest(ShoppingCart cart)
    {
        Id = Guid.NewGuid();
        OrderId = Id;
        Customer = new Customer
        {
            FirstName = "Jane",
            MiddleName = "Mary",
            LastName = "Doe"
        };
        Items = cart.Items.Select(li => new LineItem
        {
            ProductId = li.Key,
            Quantity = li.Value.Quantity,
            ListPrice = li.Value.ListPrice,
            ProductName = li.Value.ProductName
        }).ToList();
        Status = Status.New;
    }

    public Guid OrderId { get; set; }

    public List<LineItem> Items { get; set; }

    public Status Status { get; set; }

    public decimal Total
    {
        get { return Items.Sum(li => li.Subtotal); }
    }

    public Customer Customer { get; set; }
}

public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
}

public class LineItem
{
    public int Quantity { get; set; }
    public decimal ListPrice { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Subtotal => Quantity * ListPrice;
}