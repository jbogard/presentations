#region Example 4

using Data;
using FastEndpoints;
using Shouldly;
using WebApp.Endpoints.Todo;
using Xunit.Abstractions;

namespace IntegrationTests.Endpoints.Todo;

public class PostTests(WebAppFixture App, ITestOutputHelper Output)
    : WebAppTestBase(App, Output)
{
    [Fact]
    public async Task Post_updates_with_weather()
    {
        // Arrange
        var request = new PostEndpoint.Request
        {
            Name = "New Todo Item",
            IsComplete = false
        };
        
        // Act
        var (response, result) = await App.Client.POSTAsync<PostEndpoint, PostEndpoint.Request, TodoItem>(request);
        
        // Assert
        response.IsSuccessStatusCode.ShouldBeTrue();
        result.Name.ShouldContain(request.Name);
        result.Name.ShouldContain(WebAppFixture.FakeWeatherForecastClient.WeatherForecast.Summary);
        // Considering other tests might have added items
    }
}
#endregion