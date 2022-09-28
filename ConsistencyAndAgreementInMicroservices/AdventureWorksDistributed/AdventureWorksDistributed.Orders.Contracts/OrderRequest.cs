namespace AdventureWorksDistributed.Orders.Contracts;

public class OrderRequestSummary
{
    public Guid Id { get; set; }
    public Status Status { get; set; }
    public string CustomerFirstName { get; set; }
    public string CustomerMiddleName { get; set; }
    public string CustomerLastName { get; set; }
    public decimal Total { get; set; }
    public List<LineItem> Items { get; set; }

    public class LineItem
    {
        public string ProductName { get; set; }
    }
}