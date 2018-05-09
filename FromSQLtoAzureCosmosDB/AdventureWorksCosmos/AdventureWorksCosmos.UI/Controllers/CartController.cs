using System.Threading.Tasks;
using AdventureWorksCosmos.Products.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksCosmos.UI
{
    public class CartController : Controller
    {
        private readonly AdventureWorks2016Context _db;

        public CartController(AdventureWorks2016Context db) => _db = db;

        public async Task<IActionResult> AddItem(int id)
        {
            var product = await _db.Product.SingleAsync(p => p.ProductId == id);
            var cart = GetCart();

            cart.IncrementQuantity(product);

            HttpContext.Session.Set("Cart", cart);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        private ShoppingCart GetCart() 
            => HttpContext.Session.Get<ShoppingCart>("Cart") ?? new ShoppingCart();
    }
}