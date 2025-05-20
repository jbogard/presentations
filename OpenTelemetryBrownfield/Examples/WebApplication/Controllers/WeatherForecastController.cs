using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly WeatherContext _dbContext;

    public WeatherForecastController(WeatherContext dbContext) => _dbContext = dbContext;

    [HttpGet]
    public Task<List<WeatherForecast>> Get() => _dbContext.Forecasts.ToListAsync();

    [HttpGet("today")]
    public async Task<WeatherForecast> GetToday()
    {
        var forecastCount = await _dbContext.Forecasts.CountAsync();

        var rng = new Random();

        return await _dbContext.Forecasts.Skip(rng.Next(forecastCount)).FirstAsync();
    }
}