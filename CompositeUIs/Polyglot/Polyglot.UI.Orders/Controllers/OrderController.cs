using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyglot.Orders.Processor.Messages;
using Polyglot.UI.Orders.Models;

namespace Polyglot.UI.Orders.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Checkout/

        public ActionResult Edit(Guid id)
        {
            using (var session = MvcApplication.DocumentStore.OpenSession())
            {
                var order = session.Load<OrderRequest>(id);

                var form = new OrderRequestForm
                {
                    Id = order.Id,
                    Total = order.Total,
                    Items = order.Items.Select(item => new OrderRequestForm.LineItem()
                    {
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        ListPrice = item.ListPrice,
                        Subtotal = item.Subtotal
                    }).ToList()
                };

                return View(form);
            }
        }

        [HttpPost]
        public ActionResult Edit(OrderRequestForm form)
        {
            using (var session = MvcApplication.DocumentStore.OpenSession())
            {
                var order = session.Load<OrderRequest>(form.Id);

                order.Customer = new Customer
                {
                    FirstName = form.CustomerFirstName,
                    LastName = form.CustomerLastName
                };

                order.Status = Status.Submitted;

                session.SaveChanges();

                return RedirectToAction("Success", new {id = form.Id});
            }
        }

        public ActionResult Success(Guid id)
        {
            return View(new OrderConfirmModel{ OrderRequestId = id});
        }

        public ActionResult Show(Guid id)
        {
            using (var session = MvcApplication.DocumentStore.OpenSession())
            {
                var order = session.Load<OrderRequest>(id);

                var model = new OrderShowModel
                {
                    Id = order.Id,
                    Status = order.Status,
                    Total = order.Total
                };

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Approve(Guid id)
        {
            MvcApplication.Bus.Send(new ApproveOrderMessage {OrderId = id});

            return RedirectToAction("Show", new {id});
        }

    }
}
