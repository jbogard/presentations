namespace AdventureWorksDistributed.Orders.Contracts;

public enum Status
{
    New = 0,
    Submitted = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4,
    Completed = 5
}