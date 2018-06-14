using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorksCosmos.UI.Models.Inventory;
using AdventureWorksCosmos.UI.Models.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksCosmos.UI.Controllers
{
    //public class OrderController : Controller
    //{
    //    private readonly IDocumentDBRepository<OrderRequest> _orderRepository;
    //    private readonly IDocumentDBRepository<Stock> _stockRepository;

    //    public OrderController(IDocumentDBRepository<OrderRequest> orderRepository, IDocumentDBRepository<Stock> stockRepository)
    //    {
    //        _orderRepository = orderRepository;
    //        _stockRepository = stockRepository;
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> Approve(Guid id)
    //    {
    //        var orderRequest = await _orderRepository.GetItemAsync(id);

    //        orderRequest.Approve();

    //        await _orderRepository.UpdateItemAsync(orderRequest);

    //        foreach (var lineItem in orderRequest.Items)
    //        {
    //            var stock = (await _stockRepository
    //                .GetItemsAsync(s => s.ProductId == lineItem.ProductId))
    //                .FirstOrDefault();
    //            if (stock == null)
    //            {
    //                stock = new Stock
    //                {
    //                    ProductId = lineItem.ProductId,
    //                    QuantityAvailable = 100
    //                };
    //                await _stockRepository.CreateItemAsync(stock);
    //            }

    //            stock.QuantityAvailable -= lineItem.Quantity;

    //            await _stockRepository.UpdateItemAsync(stock);
    //        }

    //        return RedirectToPage("/Orders/Show", new {id});
    //    }
    //}

    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Guid id)
        {
            await _mediator.Send(new ApproveOrderRequest {Id = id});

            return RedirectToPage("/Orders/Show", new {id});
        }
    }

    public class ApproveOrderRequest : IRequest
    {
        public Guid Id { get; set; }
    }

    public class ApproveOrderHandler : IRequestHandler<ApproveOrderRequest>
    {
        private readonly IDocumentDBRepository<OrderRequest> _orderRepository;

        public ApproveOrderHandler(IDocumentDBRepository<OrderRequest> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Unit> Handle(ApproveOrderRequest request, CancellationToken cancellationToken)
        {
            var orderRequest = await _orderRepository.GetItemAsync(request.Id);

            orderRequest.Approve();

            await _orderRepository.UpdateItemAsync(orderRequest);

            return Unit.Value;
        }
    }
}