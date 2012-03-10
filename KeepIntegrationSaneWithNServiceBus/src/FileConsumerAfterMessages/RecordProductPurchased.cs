using NServiceBus;

namespace FileConsumerAfterMessages
{
    public class RecordProductPurchased : ICommand
    {
        public string TransactionId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string Sku { get; set; }
    }
}