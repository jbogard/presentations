using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using OrderProcessing.Messages;

namespace WebApp.Controllers
{

    public class OrdersContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
    }

    public class Order
    {
        public Guid Id { get; set; }
    }

    public class OrdersController : Controller
    {
        private readonly IMessageSession _bus;
        private readonly OrdersContext _dbContext;
        private static int _orderId = 1;

        public OrdersController(IMessageSession bus, OrdersContext dbContext)
        {
            _bus = bus;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var order = CreateOrderFromCart();

            // Save order details to DB
            _dbContext.Orders.Add(order);

            // Send commmand to kick off process
            await _bus.Send(new ProcessOrderCommand
            {
                OrderId = order.Id
            });

            return RedirectToAction(nameof(ThankYou));
        }

        private Order CreateOrderFromCart()
        {
            return null;
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}