using System;
using System.Collections.Generic;

namespace AdventureWorksDistributed.Products.Models
{
    public partial class AddressType
    {
        public AddressType()
        {
            BusinessEntityAddress = new HashSet<BusinessEntityAddress>();
        }

        public int AddressTypeId { get; set; }
        public string Name { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<BusinessEntityAddress> BusinessEntityAddress { get; set; }
    }
}
