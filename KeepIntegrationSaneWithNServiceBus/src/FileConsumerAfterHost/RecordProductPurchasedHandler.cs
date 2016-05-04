using System.Data.SqlClient;
using FileConsumerAfterMessages;
using NServiceBus;

namespace FileConsumerAfterHost
{
    public class RecordProductPurchasedHandler : IHandleMessages<RecordProductPurchased>
    {
        private static readonly string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=KeepingIntegrationsSane;Integrated Security=SSPI;";

        public void Handle(RecordProductPurchased message)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand(
                    "INSERT ProductPurchased" +
                    "(TransactionId, Quantity, Price, Sku)" +
                    " VALUES (@TransactionId, @Quantity, @Price, @Sku)", conn, tx))
                {
                    cmd.Parameters.AddWithValue("TransactionId", message.TransactionId);
                    cmd.Parameters.AddWithValue("Quantity", message.Quantity);
                    cmd.Parameters.AddWithValue("Price", message.Price);
                    cmd.Parameters.AddWithValue("Sku", message.Sku);
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }

        }
    }
}