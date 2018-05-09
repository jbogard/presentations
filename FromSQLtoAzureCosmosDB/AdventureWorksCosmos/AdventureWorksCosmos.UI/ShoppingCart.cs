using System;
using System.Collections.Generic;
using System.Linq;
using AdventureWorksCosmos.Products.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AdventureWorksCosmos.UI
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Items = Items ?? new Dictionary<int, LineItem>();
        }

        public decimal Total => Items.Sum(li => li.Value.Subtotal);

        public Dictionary<int, LineItem> Items { get; set; }

        public void IncrementQuantity(Product product)
        {
            if (!Items.ContainsKey(product.ProductId))
            {
                Items[product.ProductId] = new LineItem(product);
            }
            LineItem lineItem = Items[product.ProductId];
            lineItem.Quantity++;
        }
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }

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
}