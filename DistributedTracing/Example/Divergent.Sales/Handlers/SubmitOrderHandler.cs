using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Divergent.Sales.Data.Context;
using Divergent.Sales.Data.Models;
using Divergent.Sales.Messages.Commands;
using Divergent.Sales.Messages.Events;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Divergent.Sales.Handlers
{
    public class SubmitOrderHandler : IHandleMessages<SubmitOrderCommand>
    {
        private readonly SalesContext _db;
        private readonly ILogger<SubmitOrderHandler> _log;

        public SubmitOrderHandler(SalesContext db, ILogger<SubmitOrderHandler> log)
        {
            _db = db;
            _log = log;
        }

        public async Task Handle(SubmitOrderCommand message, IMessageHandlerContext context)
        {
            _log.LogInformation("Handle SubmitOrderCommand");

            var items = new List<Item>();

            var products = _db.Products.ToList();

            message.Products.ForEach(p => items.Add(new Item
            {
                Product = products.Single(s => s.Id == p)
            }));

            var order = new Data.Models.Order
            {
                CustomerId = message.CustomerId,
                DateTimeUtc = DateTime.UtcNow,
                Items = items,
                State = "New"
            };

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            // Publish event
            await context.Publish(new OrderSubmittedEvent
            {
                OrderId = order.Id,
                CustomerId = message.CustomerId,
                Products = message.Products,
            });
        }
    }
}
