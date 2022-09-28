using AdventureWorksDistributed.Orders.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksDistributed.UI.Controllers;

public class OrderController : Controller
{
    private readonly IOrdersService _ordersService;

    public OrderController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpPost]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _ordersService.Approve(id);

        return RedirectToPage("/Orders/Show", new { id });
    }

    [HttpPost]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _ordersService.Reject(id);

        return RedirectToPage("/Orders/Show", new { id });
    }
}