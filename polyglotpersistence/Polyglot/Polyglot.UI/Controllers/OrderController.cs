using System.Web.Mvc;
using Polyglot.Cart;
using ServiceStack.Redis;

namespace Polyglot.UI.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/

        public ActionResult Checkout()
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
