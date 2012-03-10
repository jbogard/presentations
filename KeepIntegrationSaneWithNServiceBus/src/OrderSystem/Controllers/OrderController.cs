using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using OrderSystem.Models;

namespace OrderSystem.Controllers
{
    public class OrderController : Controller
    {
        private static readonly string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=KeepingIntegrationsSane;Integrated Security=SSPI;";

        //
        // GET: /Order/

        public ActionResult New()
        {
            return View(new OrderForm());
        }

        [HttpPost]
        public ActionResult New(OrderForm form)
        {
            var order = new Order
            {
                Name = form.Name,
                Amount = form.Amount,
            };

            var orderSvc = new Orders.OrderServiceClient();

            order.OrderId = orderSvc.CreateOrder(order.Name, order.Amount);

            var shippingSvc = new Shipping.ShippingServiceClient();

            bool success = shippingSvc.ProcessOrderForShipping(order.OrderId);

            order.Success = success;

            SaveOrder(order);

            return RedirectToAction("Show", new { order.OrderId });
        }

        public ActionResult Show(int orderId)
        {
            var order = GetOrder(orderId);

            return View(order);
        }

        private Order GetOrder(int orderId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("SELECT OrderId, Name, Amount, Success FROM OrdersPlaced WHERE OrderId = @OrderId", conn, tx))
                {
                    cmd.Parameters.AddWithValue("OrderId", orderId);

                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.Read())
                        {
                            var order = new Order
                            {
                                OrderId = (int) reader[0],
                                Name = (string) reader[1],
                                Amount = (decimal) reader[2],
                                Success = (bool) reader[3],
                            };

                            return order;
                        }
                    }
                }
            }

            return null;
        }


        private void SaveOrder(Order order)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("INSERT OrdersPlaced (OrderId, Name, Amount, Success) VALUES (@OrderId, @Name, @Amount, @Success)", conn, tx))
                {
                    cmd.Parameters.AddWithValue("OrderId", order.OrderId);
                    cmd.Parameters.AddWithValue("Name", order.Name);
                    cmd.Parameters.AddWithValue("Amount", order.Amount);
                    cmd.Parameters.AddWithValue("Success", order.Success);
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }

        }
    }
}
