using System.Dynamic;
using WorkerService.Messages;
using static System.Text.Json.JsonSerializer;

namespace WorkerService;

public class GetTemperatureHandler : IHandleMessages<GetTemperature>
{
    private readonly ILogger<GetTemperatureHandler> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public GetTemperatureHandler(ILogger<GetTemperatureHandler> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task Handle(GetTemperature message, IMessageHandlerContext context)
    {
        var httpClient = _httpClientFactory.CreateClient("web");

        var content = await httpClient.GetStringAsync("/weatherforecast/today", context.CancellationToken);

        dynamic json = Deserialize<ExpandoObject>(content);

        var temp = (int)json.temperatureF.GetInt32();

        await context.Reply(new GetTemperatureResponse
        {
            Value = temp
        });
    }
}