using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using OrderProcessing.Messages;

namespace WebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IMessageSession _bus;
        private static int _orderId = 1;

        public OrdersController(IMessageSession bus)
        {
            _bus = bus;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            // Save order details to DB
            // dbContext.Orders.Add(order);

            // Send commmand to kick off process
            await _bus.Send(new ProcessOrderCommand
            {
                OrderId = _orderId++
            });

            return RedirectToAction(nameof(ThankYou));
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}