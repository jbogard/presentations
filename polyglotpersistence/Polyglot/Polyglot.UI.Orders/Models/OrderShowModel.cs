using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polyglot.UI.Orders.Models
{
    public class OrderShowModel
    {
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public Status Status { get; set; }
    }
}