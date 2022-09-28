using NServiceBus;

namespace AdventureWorksDistributed.Fulfillment.Endpoint
{
    public class OrderFulfillment : ContainSagaData
    {
        public Guid OrderId { get; set; }
        public bool IsCancelled { get; set; }
        public bool CancelOrderRequested { get; set; }
        public List<LineItem> LineItems { get; set; }
        public bool OrderApproved { get; set; }
        public bool OrderRejected { get; set; }

        public class LineItem
        {
            public int ProductId { get; set; }
            public int AmountRequested { get; set; }
            public bool StockConfirmed { get; set; }
            public bool StockReturnRequested { get; set; }
        }
    }
}