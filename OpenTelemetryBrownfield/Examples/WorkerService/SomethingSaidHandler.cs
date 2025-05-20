using System;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using NServiceBus;
using WebApplication.Messages;
using static System.Text.Json.JsonSerializer;
using WorkerService.Messages;

namespace WorkerService;

public class SomethingSaidHandler : IHandleMessages<SomethingSaid>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SomethingSaidHandler(IHttpClientFactory httpClientFactory) 
        => _httpClientFactory = httpClientFactory;

    public async Task Handle(SomethingSaid message, IMessageHandlerContext context)
    {
        var httpClient = _httpClientFactory.CreateClient("web");
        var content = await httpClient.GetStringAsync("/weatherforecast/today", context.CancellationToken);

        dynamic json = Deserialize<ExpandoObject>(content);

        var temp = (int)json.temperatureF.GetInt32();

        await context.Publish(new TemperatureRead
        {
            Id = message.Id,
            Value = temp
        });
    }
}