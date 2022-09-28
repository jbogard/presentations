using AdventureWorksDistributed.Products.Models;

namespace AdventureWorksDistributed.Cart.Contracts;

public class LineItem
{
    public LineItem() { }

    public LineItem(Product product)
    {
        ProductID = product.ProductId;
        ProductName = product.Name;
        ListPrice = product.ListPrice;
    }

    public int Quantity { get; set; }

    public decimal ListPrice { get; set; }

    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public decimal Subtotal => Quantity * ListPrice;
}