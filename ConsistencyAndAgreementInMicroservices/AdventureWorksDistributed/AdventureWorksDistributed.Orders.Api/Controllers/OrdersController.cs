using AdventureWorksDistributed.Cart.Contracts;
using AdventureWorksDistributed.Core.Infrastructure;
using AdventureWorksDistributed.Orders.Api.Models;
using AdventureWorksDistributed.Orders.Contracts;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace AdventureWorksDistributed.Orders.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IDocumentDbRepository<OrderRequest> _orderRepository;
        private readonly IMessageSession _messageSession;

        public OrdersController(IDocumentDbRepository<OrderRequest> orderRepository, IMessageSession messageSession)
        {
            _orderRepository = orderRepository;
            _messageSession = messageSession;
        }

        [HttpGet("{id}")]
        public async Task<OrderRequestSummary> Get(Guid id)
        {
            var order = await _orderRepository.GetItemAsync(id, id.ToString());

            var orderSummary = new OrderRequestSummary
            {
                Id = order.Id,
                Status = order.Status,
                Total = order.Total,
                CustomerFirstName = order.Customer.FirstName,
                CustomerMiddleName = order.Customer.MiddleName,
                CustomerLastName = order.Customer.LastName,
                Items = order.Items.Select(item => new OrderRequestSummary.LineItem
                    {
                        ProductName = item.ProductName
                    })
                    .ToList()
            };

            return orderSummary;
        }

        [HttpPost]
        public async Task<Guid> Post(ShoppingCart cart)
        {
            var orderRequest = new OrderRequest(cart);

            await _orderRepository.CreateItemAsync(orderRequest);

            await _messageSession.Publish(new OrderCreated
            {
                OrderId = orderRequest.Id,
                LineItems = orderRequest.Items
                    .Select(item => new OrderCreated.LineItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    })
                    .ToList()
            });

            return orderRequest.Id;
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var orderRequest = await _orderRepository.GetItemAsync(id, id.ToString());

            if (orderRequest.Status == Status.Approved)
                return Ok();

            if (orderRequest.Status == Status.Rejected)
            {
                return BadRequest(new
                {
                    Message = "Cannot approve a rejected order."
                });
            }

            orderRequest.Status = Status.Approved;

            await _orderRepository.UpdateItemAsync(orderRequest);

            await _messageSession.Publish(new OrderApproved
            {
                OrderId = orderRequest.Id
            });

            return Ok();
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id)
        {
            var orderRequest = await _orderRepository.GetItemAsync(id, id.ToString());

            if (orderRequest.Status == Status.Rejected)
            {
                return Ok();
            }

            if (orderRequest.Status == Status.Approved)
            {
                return BadRequest(new
                {
                    Message = "Cannot reject an approved order."
                });
            }

            if (orderRequest.Status == Status.Approved)
            {
                return BadRequest(new
                {
                    Message = "Cannot reject a completed order."
                });
            }

            orderRequest.Status = Status.Rejected;
            
            await _orderRepository.UpdateItemAsync(orderRequest);

            await _messageSession.Publish(new OrderRejected
            {
                OrderId = orderRequest.Id
            });

            return Ok();
        }

    }
}
