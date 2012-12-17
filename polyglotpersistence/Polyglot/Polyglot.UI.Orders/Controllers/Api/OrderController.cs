using System;
using System.Linq;
using System.Web.Http;
using Polyglot.Orders;
using Polyglot.UI.Orders.Models;

namespace Polyglot.UI.Orders.Controllers.Api
{

    public class OrderController : ApiController
    {
        public OrderResponse Post(Order order)
        {
            using (var session = MvcApplication.DocumentStore.OpenSession())
            {
                var request = new OrderRequest
                {
                    Items = order.Items.Select(li => new Models.LineItem
                    {
                        ProductID = li.ProductID,
                        Quantity = li.Quantity,
                        ListPrice = li.ListPrice,
                        ProductName = li.ProductName
                    }).ToList(),
                    Status = Status.New
                };

                session.Store(request);

                session.SaveChanges();

                return new OrderResponse
                {
                    Id = request.Id,
                    CheckoutUri = "http://localhost:58800/order/edit/" + request.Id.ToString()
                };
            }
        }
    }
}
