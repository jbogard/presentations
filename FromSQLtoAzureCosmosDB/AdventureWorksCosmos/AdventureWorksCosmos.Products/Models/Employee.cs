using System;
using System.Collections.Generic;

namespace AdventureWorksCosmos.Products.Models
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeeDepartmentHistory = new HashSet<EmployeeDepartmentHistory>();
            EmployeePayHistory = new HashSet<EmployeePayHistory>();
            JobCandidate = new HashSet<JobCandidate>();
            PurchaseOrderHeader = new HashSet<PurchaseOrderHeader>();
        }

        public int BusinessEntityId { get; set; }
        public string NationalIdnumber { get; set; }
        public string LoginId { get; set; }
        public short? OrganizationLevel { get; set; }
        public string JobTitle { get; set; }
        public DateTime BirthDate { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public DateTime HireDate { get; set; }
        public bool? SalariedFlag { get; set; }
        public short VacationHours { get; set; }
        public short SickLeaveHours { get; set; }
        public bool? CurrentFlag { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Person BusinessEntity { get; set; }
        public SalesPerson SalesPerson { get; set; }
        public ICollection<EmployeeDepartmentHistory> EmployeeDepartmentHistory { get; set; }
        public ICollection<EmployeePayHistory> EmployeePayHistory { get; set; }
        public ICollection<JobCandidate> JobCandidate { get; set; }
        public ICollection<PurchaseOrderHeader> PurchaseOrderHeader { get; set; }
    }
}
