using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using OrderProcessorMessages;
using OrderSystem.Models;

namespace OrderSystem.Controllers
{
    public class OrderAfterController : Controller
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
            var order = new OrderAfter
            {
                Name = form.Name,
                Amount = form.Amount,
                ClientOrderId = Guid.NewGuid()
            };

            SaveOrder(order);

            var message = new PlaceOrder
            {
                Name = order.Name, 
                Amount = order.Amount, 
                ClientOrderId = order.ClientOrderId
            };

            MvcApplication.Bus.Send(message);

            return RedirectToAction("Show", new { order.ClientOrderId });
        }

        public ActionResult Show(Guid clientOrderId)
        {
            var order = GetOrder(clientOrderId);

            return View(order);
        }

        private OrderAfter GetOrder(Guid clientOrderId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("SELECT OrderId, Name, Amount, Success, ClientOrderId FROM OrdersPlaced WHERE ClientOrderId = @ClientOrderId", conn, tx))
                {
                    cmd.Parameters.AddWithValue("ClientOrderId", clientOrderId);

                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.Read())
                        {
                            var order = new OrderAfter
                            {
                                OrderId = (int) reader[0],
                                Name = (string) reader[1],
                                Amount = (decimal) reader[2],
                                Success = (bool) reader[3],
                                ClientOrderId = (Guid) reader[4],
                            };

                            return order;
                        }
                    }
                }
            }

            return null;
        }


        private void SaveOrder(OrderAfter order)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                using (var cmd = new SqlCommand("INSERT OrdersPlaced (OrderId, Name, Amount, Success, ClientOrderId) VALUES (@OrderId, @Name, @Amount, @Success, @ClientOrderId)", conn, tx))
                {
                    cmd.Parameters.AddWithValue("OrderId", order.OrderId);
                    cmd.Parameters.AddWithValue("Name", order.Name);
                    cmd.Parameters.AddWithValue("Amount", order.Amount);
                    cmd.Parameters.AddWithValue("Success", order.Success);
                    cmd.Parameters.AddWithValue("ClientOrderId", order.ClientOrderId);
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }

        }
    }
}
