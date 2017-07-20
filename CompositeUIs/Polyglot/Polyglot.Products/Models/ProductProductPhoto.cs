using System;
using System.Collections.Generic;

namespace Polyglot.Products.Models
{
    public class ProductProductPhoto
    {
        public int ProductID { get; set; }
        public int ProductPhotoID { get; set; }
        public bool Primary { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductPhoto ProductPhoto { get; set; }
    }
}
