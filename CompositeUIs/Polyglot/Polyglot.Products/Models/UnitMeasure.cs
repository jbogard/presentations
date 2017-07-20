using System;
using System.Collections.Generic;

namespace Polyglot.Products.Models
{
    public class UnitMeasure
    {
        public UnitMeasure()
        {
            this.Products = new List<Product>();
            this.Products1 = new List<Product>();
        }

        public string UnitMeasureCode { get; set; }
        public string Name { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Product> Products1 { get; set; }
    }
}
