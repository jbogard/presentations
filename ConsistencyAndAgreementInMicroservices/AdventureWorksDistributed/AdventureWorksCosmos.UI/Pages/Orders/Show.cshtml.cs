using AdventureWorksDistributed.Orders.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventureWorksDistributed.UI.Pages.Orders;

public class ShowModel : PageModel
{
    private readonly IOrdersService _ordersService;

    public ShowModel(IOrdersService ordersService) => _ordersService = ordersService;

    public async Task OnGet(Guid id)
    {
        Order = await _ordersService.Get(id);
    }

    public OrderRequestSummary Order { get; set; }
}