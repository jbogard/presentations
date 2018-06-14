using System;
using System.Collections.Generic;
using System.Linq;
using AdventureWorksCosmos.UI.Infrastructure;

namespace AdventureWorksCosmos.UI.Models.Orders
{
    public enum Status
    {
        New = 0,
        Submitted = 1,
        Approved = 2
    }

    public class OrderRequest : AggregateBase
    {
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
    }

    public class LineItem
    {
        public int Quantity { get; set; }
        public decimal ListPrice { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Subtotal => Quantity * ListPrice;
    }
}