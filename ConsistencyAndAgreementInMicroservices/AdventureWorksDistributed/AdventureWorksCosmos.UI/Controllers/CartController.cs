using AdventureWorksDistributed.Cart.Contracts;
using AdventureWorksDistributed.Orders.Contracts;
using AdventureWorksDistributed.Products.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksDistributed.UI.Controllers;

public class CartController : Controller
{
    private readonly AdventureWorks2016Context _db;
    private readonly IOrdersService _ordersService;

    public CartController(AdventureWorks2016Context db, IOrdersService ordersService)
    {
        _db = db;
        _ordersService = ordersService;
    }

    public async Task<IActionResult> AddItem(int id)
    {
        var product = await _db.Product.SingleAsync(p => p.ProductId == id);
        var cart = GetCart();

        cart.IncrementQuantity(product);

        HttpContext.Session.Set("Cart", cart);

        return Redirect(Request.Headers["Referer"].ToString());
    }

    public async Task<IActionResult> Checkout()
    {
        var cart = GetCart();

        var response = await _ordersService.Post(cart);

        cart.Items.Clear();

        HttpContext.Session.Set("Cart", cart);

        return RedirectToPage("/Orders/Show", new {id = response});
    }

    private ShoppingCart GetCart() 
        => HttpContext.Session.Get<ShoppingCart>("Cart") ?? new ShoppingCart();
}