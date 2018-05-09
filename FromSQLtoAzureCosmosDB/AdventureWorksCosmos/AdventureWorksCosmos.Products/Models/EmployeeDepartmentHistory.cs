using System;
using System.Collections.Generic;

namespace AdventureWorksCosmos.Products.Models
{
    public partial class EmployeeDepartmentHistory
    {
        public int BusinessEntityId { get; set; }
        public short DepartmentId { get; set; }
        public byte ShiftId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Employee BusinessEntity { get; set; }
        public Department Department { get; set; }
        public Shift Shift { get; set; }
    }
}
