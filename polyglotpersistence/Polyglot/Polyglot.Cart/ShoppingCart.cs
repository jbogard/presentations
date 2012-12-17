using System;
using System.Collections.Generic;
using System.Linq;

namespace Polyglot.Cart
{
    [Serializable]
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Items = Items ?? new Dictionary<int, LineItem>();
        }

        public decimal Total
        {
            get { return Items.Sum(li => li.Value.Subtotal); }
        }
        
        public Dictionary<int, LineItem> Items { get; set; }

        public void IncrementQuantity(dynamic product)
        {
            if (!Items.ContainsKey(product.ProductID))
            {
                Items[product.ProductID] = new LineItem(product);
            }
            LineItem lineItem = Items[product.ProductID];
            lineItem.Quantity++;
        }
    }

    [Serializable]
    public class LineItem
    {
        public LineItem(dynamic product)
        {
            ProductID = product.ProductID;
            ProductName = product.Name;
            ListPrice = product.ListPrice;
        }

        public int Quantity { get; set; }

        public decimal ListPrice { get; set; }

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Subtotal { get { return Quantity*ListPrice; } }
    }
}
