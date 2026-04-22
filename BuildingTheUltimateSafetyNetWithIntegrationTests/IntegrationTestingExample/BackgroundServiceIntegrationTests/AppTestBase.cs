using BackgroundServiceIntegrationTests;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Communication.HandlerIntegrationTests;

[Collection(nameof(BackgroundServiceApp))]
public abstract class AppTestBase : IDisposable
{
    protected BackgroundServiceApp App { get; }
    protected ITestOutputHelper Output { get; }

    protected AppTestBase(BackgroundServiceApp app, ITestOutputHelper output)
    {
        App = app;
        Output = output;
        app.SetOutputHelper(output);
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
}
