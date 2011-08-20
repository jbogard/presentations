namespace OrderSystem.Models
{
    public class OrderForm
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool Success { get; set; }
    }
}