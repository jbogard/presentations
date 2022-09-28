using AdventureWorksDistributed.Cart.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksDistributed.UI.ViewComponents;

public class CartWidgetViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var cart = HttpContext.Session.Get<ShoppingCart>("Cart") ?? new ShoppingCart();

        return View(cart);
    }
}