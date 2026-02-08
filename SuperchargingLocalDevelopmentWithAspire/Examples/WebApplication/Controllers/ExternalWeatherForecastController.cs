using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers;

#region External API client
[ApiController]
[Route("[controller]")]
public class ExternalWeatherForecastController(
    ExternalApi.ExternalApiClient client) : ControllerBase
{

    [HttpGet]
    public Task<ExternalApi.ExternalWeatherForecast[]> Get() 
        => client.GetWeatherForecastAsync();

}
#endregion