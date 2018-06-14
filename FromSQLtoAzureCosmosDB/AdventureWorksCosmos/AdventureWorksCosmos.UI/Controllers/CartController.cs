using System;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorksCosmos.Products.Models;
using AdventureWorksCosmos.UI.Models.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksCosmos.UI
{
    public class CartController : Controller
    {
        private readonly AdventureWorks2016Context _db;
        private readonly IDocumentDBRepository<OrderRequest> _docDbRepository;

        public CartController(AdventureWorks2016Context db, IDocumentDBRepository<OrderRequest> docDbRepository)
        {
            _db = db;
            _docDbRepository = docDbRepository;
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

            var request = new OrderRequest
            {
                Id = Guid.NewGuid(),
                Customer = new Models.Orders.Customer
                {
                    FirstName = "Jane",
                    MiddleName = "Mary",
                    LastName = "Doe"
                },
                Items = cart.Items.Select(li => new Models.Orders.LineItem
                {
                    ProductId = li.Key,
                    Quantity = li.Value.Quantity,
                    ListPrice = li.Value.ListPrice,
                    ProductName = li.Value.ProductName
                }).ToList(),
                Status = Status.New
            };

            var doc = await _docDbRepository.CreateItemAsync(request);

            cart.Items.Clear();

            HttpContext.Session.Set("Cart", cart);

            return RedirectToPage("/Orders/Show", new {id = doc.Id});
        }

        private ShoppingCart GetCart() 
            => HttpContext.Session.Get<ShoppingCart>("Cart") ?? new ShoppingCart();
    }
} 