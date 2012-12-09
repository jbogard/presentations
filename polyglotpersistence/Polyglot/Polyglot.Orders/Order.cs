using System;
using System.Collections.Generic;

namespace Polyglot.Orders
{
    public class Order
    {
        public List<LineItem> Items { get; set; }
    }

    public class OrderResponse
    {
        public string CheckoutUri { get; set; }
        public Guid Id { get; set; }
    }

    public class LineItem
    {
        public int Quantity { get; set; }
        public decimal ListPrice { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }

}
