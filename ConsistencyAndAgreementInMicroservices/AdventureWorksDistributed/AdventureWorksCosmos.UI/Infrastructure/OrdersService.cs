using AdventureWorksDistributed.Cart.Contracts;
using AdventureWorksDistributed.Orders.Contracts;

namespace AdventureWorksDistributed.UI.Infrastructure;

public class OrdersService : IOrdersService
{
    private readonly HttpClient _client;

    public OrdersService(HttpClient client)
    {
        _client = client;
    }
    public async Task<OrderRequestSummary> Get(Guid id)
    {
        return await _client.GetFromJsonAsync<OrderRequestSummary>($"/api/orders/{id}");
    }

    public async Task<Guid> Post(ShoppingCart cart)
    {
        var response = await _client.PostAsJsonAsync("/api/orders", cart);

        response.EnsureSuccessStatusCode();

        var orderId = await response.Content.ReadFromJsonAsync<Guid>();

        return orderId;
    }

    public async Task Approve(Guid id)
    {
        var response = await _client.PostAsync($"/api/orders/{id}/approve", null);

        response.EnsureSuccessStatusCode();
    }

    public async Task Reject(Guid id)
    {
        var response = await _client.PostAsync($"/api/orders/{id}/reject", null);

        response.EnsureSuccessStatusCode();
    }
}