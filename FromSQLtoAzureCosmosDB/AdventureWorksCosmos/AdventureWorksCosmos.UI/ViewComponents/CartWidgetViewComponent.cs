using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksCosmos.UI.ViewComponents
{
    public class CartWidgetViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<ShoppingCart>("Cart") ?? new ShoppingCart();

            return View(cart);
        }
    }
}