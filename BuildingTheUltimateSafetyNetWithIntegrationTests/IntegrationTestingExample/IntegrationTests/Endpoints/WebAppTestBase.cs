#region Example 2

using Data;
using FastEndpoints.Testing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Endpoints;

public class WebAppTestBase : TestBase<WebAppFixture>, IDisposable
{
    protected WebAppFixture App { get; }

    protected WebAppTestBase(WebAppFixture app)
    {
        App = app;
        app.SetOutputHelper(Output);
    }
    
    public void Dispose()
    {
        App.ClearOutputHelper();
    }

    public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = App.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WebAppDbContext>();

        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () => await action(scope.ServiceProvider));
    }

    public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = App.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WebAppDbContext>();

        var strategy = dbContext.Database.CreateExecutionStrategy();

        var result = await strategy.ExecuteAsync(async () => await action(scope.ServiceProvider));

        return result;
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<WebAppDbContext, Task<T>> action) =>
        ExecuteScopeAsync(sp => action(sp.GetRequiredService<WebAppDbContext>()));

    public Task ExecuteDbContextAsync(Func<WebAppDbContext, Task> action) =>
        ExecuteScopeAsync(sp => action(sp.GetRequiredService<WebAppDbContext>()));

    #region Example 5
    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        return ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();
    
            return mediator.Send(request);
        });
    }
    #endregion
}
#endregion