using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class ExternalWeatherForecastController(ExternalApi.ExternalApiClient client) : ControllerBase
{

    [HttpGet]
    public Task<ExternalApi.ExternalWeatherForecast[]> Get() => client.GetWeatherForecastAsync();

}