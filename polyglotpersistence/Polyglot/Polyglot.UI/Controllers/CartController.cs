using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyglot.Cart;
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

        private ShoppingCart GetCart()
        {
            return (ShoppingCart)Session["Cart"] ?? new ShoppingCart();
        }
    }
}
