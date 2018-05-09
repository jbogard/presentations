using System;
using System.Collections.Generic;

namespace AdventureWorksCosmos.Products.Models
{
    public partial class UnitMeasure
    {
        public UnitMeasure()
        {
            BillOfMaterials = new HashSet<BillOfMaterials>();
            ProductSizeUnitMeasureCodeNavigation = new HashSet<Product>();
            ProductVendor = new HashSet<ProductVendor>();
            ProductWeightUnitMeasureCodeNavigation = new HashSet<Product>();
        }

        public string UnitMeasureCode { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<BillOfMaterials> BillOfMaterials { get; set; }
        public ICollection<Product> ProductSizeUnitMeasureCodeNavigation { get; set; }
        public ICollection<ProductVendor> ProductVendor { get; set; }
        public ICollection<Product> ProductWeightUnitMeasureCodeNavigation { get; set; }
    }
}
