using System;
using System.Collections.Generic;

namespace AdventureWorksCosmos.Products.Models
{
    public partial class BusinessEntity
    {
        public BusinessEntity()
        {
            BusinessEntityAddress = new HashSet<BusinessEntityAddress>();
            BusinessEntityContact = new HashSet<BusinessEntityContact>();
        }

        public int BusinessEntityId { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Person Person { get; set; }
        public Store Store { get; set; }
        public Vendor Vendor { get; set; }
        public ICollection<BusinessEntityAddress> BusinessEntityAddress { get; set; }
        public ICollection<BusinessEntityContact> BusinessEntityContact { get; set; }
    }
}
