using System;
using System.Collections.Generic;

namespace AdventureWorksDistributed.Products.Models
{
    public partial class JobCandidate
    {
        public int JobCandidateId { get; set; }
        public int? BusinessEntityId { get; set; }
        public string Resume { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Employee BusinessEntity { get; set; }
    }
}
