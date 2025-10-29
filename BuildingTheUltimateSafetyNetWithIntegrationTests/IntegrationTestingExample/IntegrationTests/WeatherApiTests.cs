#region Example 1

using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace IntegrationTests;

public class WeatherApiTests(WebApplicationFactory<Program> factory) 
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Should_return_weather_report()
    {
        // Arrange
        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/weatherforecast");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content
            .ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        content.ShouldNotBeEmpty();
    }
}

#endregion