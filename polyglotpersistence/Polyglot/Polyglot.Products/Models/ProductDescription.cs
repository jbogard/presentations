using System;
using System.Collections.Generic;

namespace Polyglot.Products.Models
{
    public class ProductDescription
    {
        public ProductDescription()
        {
            this.ProductModelProductDescriptionCultures = new List<ProductModelProductDescriptionCulture>();
        }

        public int ProductDescriptionID { get; set; }
        public string Description { get; set; }
        public System.Guid rowguid { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public virtual ICollection<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures { get; set; }
    }
}
