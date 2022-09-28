using AdventureWorksDistributed.Products.Models;

namespace AdventureWorksDistributed.Cart.Contracts;

public class ShoppingCart
{
    public ShoppingCart()
    {
        Items ??= new Dictionary<int, LineItem>();
    }

    public decimal Total => Items.Sum(li => li.Value.Subtotal);

    public Dictionary<int, LineItem> Items { get; set; }

    public void IncrementQuantity(Product product)
    {
        if (!Items.ContainsKey(product.ProductId))
        {
            Items[product.ProductId] = new LineItem(product);
        }

        Items[product.ProductId].Quantity++;
    }
}