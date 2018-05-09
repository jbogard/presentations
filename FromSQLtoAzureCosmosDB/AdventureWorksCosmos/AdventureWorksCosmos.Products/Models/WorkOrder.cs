using System;
using System.Collections.Generic;

namespace AdventureWorksCosmos.Products.Models
{
    public partial class WorkOrder
    {
        public WorkOrder()
        {
            WorkOrderRouting = new HashSet<WorkOrderRouting>();
        }

        public int WorkOrderId { get; set; }
        public int ProductId { get; set; }
        public int OrderQty { get; set; }
        public int StockedQty { get; set; }
        public short ScrappedQty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime DueDate { get; set; }
        public short? ScrapReasonId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Product Product { get; set; }
        public ScrapReason ScrapReason { get; set; }
        public ICollection<WorkOrderRouting> WorkOrderRouting { get; set; }
    }
}
