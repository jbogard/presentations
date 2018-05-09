using System;
using System.Collections.Generic;

namespace AdventureWorksCosmos.Products.Models
{
    public partial class WorkOrderRouting
    {
        public int WorkOrderId { get; set; }
        public int ProductId { get; set; }
        public short OperationSequence { get; set; }
        public short LocationId { get; set; }
        public DateTime ScheduledStartDate { get; set; }
        public DateTime ScheduledEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal? ActualResourceHrs { get; set; }
        public decimal PlannedCost { get; set; }
        public decimal? ActualCost { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Location Location { get; set; }
        public WorkOrder WorkOrder { get; set; }
    }
}
