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

    public class OrderRequest : DocumentBase
    {
        public List<LineItem> Items { get; set; }

        public Status Status { get; set; }

        public decimal Total
        {
            get { return Items.Sum(li => li.Subtotal); }
        }

        public Customer Customer { get; set; }

        public void Approve()
        {
            Status = Status.Approved;
            foreach (var lineItem in Items)
            {
                Send(new ItemPurchased
                {
                    ProductId = lineItem.ProductId,
                    Quantity = lineItem.Quantity,
                    Id = Guid.NewGuid()
                });
            }
        }
    }

    public class ItemPurchased : IDocumentMessage
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid Id { get; set; }
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
}