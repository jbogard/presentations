using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Polyglot.UI.Orders.Models
{
    public class OrderRequestForm
    {
        public Guid Id { get; set; }

        public List<LineItem> Items { get; set; }

        public decimal Total { get; set; }

        [DisplayName("First Name")]
        public string CustomerFirstName { get; set; }
        [DisplayName("Last Name")]
        public string CustomerLastName { get; set; }

        public class LineItem
        {
            public int Quantity { get; set; }
            public decimal ListPrice { get; set; }
            public string ProductName { get; set; }
            public decimal Subtotal { get; set; }
        }
    }
}