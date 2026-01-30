using System.Net.Http.Json;

#region External API Client

// namespace WebApplication.ExternalApi;
//
// public class ExternalApiClient(HttpClient httpClient)
// {
//     public async Task<ExternalWeatherForecast[]> GetWeatherForecastAsync(CancellationToken cancellationToken = default)
//     {
//         var forecasts = await httpClient.GetFromJsonAsync<ExternalWeatherForecast[]>("weatherforecast", cancellationToken);
//         return forecasts ?? [];
//     }
// }
//
// public record ExternalWeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

#endregion