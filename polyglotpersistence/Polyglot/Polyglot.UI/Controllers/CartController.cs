using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Polyglot.Cart;
using Polyglot.Orders;
using Polyglot.Products.Models;
using ServiceStack.Redis;

namespace Polyglot.UI.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/

        public ActionResult Widget()
        {
            var cart = GetCart();

            return PartialView(cart);
        }

        public ActionResult AddItem(int productId)
        {
            using (var context = new AdventureWorks2012Context())
            {
                var product = context.Products.Single(p => p.ProductID == productId);
                var cart = GetCart();

                cart.IncrementQuantity(product);

                Session["Cart"] = cart;

                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        public ActionResult Index()
        {
            var cart = GetCart();

            return View(cart);
        }

        public async Task<ActionResult> Checkout()
        {
            using (var client = new HttpClient())
            {
                var cart = GetCart();

                var order = new Order
                {
                    Items = cart.Items.Select(kvp => new Orders.LineItem
                    {
                        ProductID = kvp.Value.ProductID, 
                        ListPrice = kvp.Value.ListPrice,
                        Quantity = kvp.Value.Quantity,
                        ProductName = kvp.Value.ProductName
                    }).ToList()
                };

                client.BaseAddress = new Uri("http://localhost:58800/");

                var httpResponseMessage = await client.PostAsJsonAsync("api/order", order);

                var orderResponse = await httpResponseMessage.Content.ReadAsAsync<OrderResponse>();

                cart.Items.Clear();

                return Redirect(orderResponse.CheckoutUri);
            }
        }

        private ShoppingCart GetCart()
        {
            return (ShoppingCart)Session["Cart"] ?? new ShoppingCart();
        }
    }
}
