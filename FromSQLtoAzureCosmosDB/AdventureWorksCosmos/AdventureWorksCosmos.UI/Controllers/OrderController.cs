using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorksCosmos.UI.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksCosmos.UI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IDocumentDBRepository<OrderRequest> _docDbRepository;

        public OrderController(IDocumentDBRepository<OrderRequest> docDbRepository)
        {
            _docDbRepository = docDbRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string id)
        {
            var orderRequest = await _docDbRepository.GetItemAsync(id);

            orderRequest.Status = Status.Approved;

            await _docDbRepository.UpdateItemAsync(id, orderRequest);

            return RedirectToPage("/Orders/Show", new {id});
        }
    }
}