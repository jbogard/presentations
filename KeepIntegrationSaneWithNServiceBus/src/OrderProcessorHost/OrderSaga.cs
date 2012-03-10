using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Saga;
using OrderProcessorMessages;

namespace OrderProcessorHost
{
    public class OrderSaga : Saga<OrderSagaData>, 
        IAmStartedByMessages<PlaceOrder>,
        IHandleMessages<ShipOrder>
    {
        private static readonly string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=KeepingIntegrationsSane;Integrated Security=SSPI;";

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<ShipOrder>(saga => saga.ClientOrderId, msg => msg.ClientOrderId);
        }

        public void Handle(PlaceOrder message)
        {
            Data.ClientOrderId = message.ClientOrderId;

            var orderSvc = new Orders.OrderServiceClient();

            Data.OrderId = orderSvc.CreateOrder(message.Name, message.Amount);

            UpdateOrderId();

            Bus.SendLocal(new ShipOrder {ClientOrderId = Data.ClientOrderId});
        }

        public void Handle(ShipOrder message)
        {
            var shippingSvc = new Shipping.ShippingServiceClient();

            bool success = shippingSvc.ProcessOrderForShipping(Data.OrderId);

            UpdateOrderSuccess(success);

            MarkAsComplete();
        }

        private void UpdateOrderId()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("UPDATE OrdersPlaced SET OrderId = @OrderId WHERE ClientOrderId = @ClientOrderId", conn, tx))
                {
                    cmd.Parameters.AddWithValue("OrderId", Data.OrderId);
                    cmd.Parameters.AddWithValue("ClientOrderId", Data.ClientOrderId);
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }

        }

        private void UpdateOrderSuccess(bool success)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("UPDATE OrdersPlaced SET Success = @Success WHERE ClientOrderId = @ClientOrderId", conn, tx))
                {
                    cmd.Parameters.AddWithValue("Success", success);
                    cmd.Parameters.AddWithValue("ClientOrderId", Data.ClientOrderId);
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }
        }
    }
}