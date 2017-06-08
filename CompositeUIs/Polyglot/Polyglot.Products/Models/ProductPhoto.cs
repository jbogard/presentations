using System;
using System.Collections.Generic;

namespace Polyglot.Products.Models
{
    public class ProductPhoto
    {
        public ProductPhoto()
        {
            this.ProductProductPhotoes = new List<ProductProductPhoto>();
        }

        public int ProductPhotoID { get; set; }
        public byte[] ThumbNailPhoto { get; set; }
        public string ThumbnailPhotoFileName { get; set; }
        public byte[] LargePhoto { get; set; }
        public string LargePhotoFileName { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public virtual ICollection<ProductProductPhoto> ProductProductPhotoes { get; set; }
    }
}
