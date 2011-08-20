using System;

namespace FileConsumer
{
    public class ProductPurchased
    {
        public string TransactionId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string Sku { get; set; }
    }
}