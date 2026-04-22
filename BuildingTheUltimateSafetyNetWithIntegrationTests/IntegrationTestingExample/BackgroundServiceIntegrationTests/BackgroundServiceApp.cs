using Communication.HandlerIntegrationTests;
using Data;
using MartinCostello.Logging.XUnit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using Xunit;

namespace BackgroundServiceIntegrationTests;

[CollectionDefinition(nameof(BackgroundServiceApp))]
public class BackgroundServiceAppCollection : ICollectionFixture<BackgroundServiceApp> { }

public class BackgroundServiceApp : IAsyncLifetime, ITestOutputHelperAccessor
{
    private PostgreSqlContainer _dbContainer = null!;
    private ApplicationFactory<Program>? _app;

    public ITestOutputHelper? OutputHelper { get; set; }
    public IServiceProvider Services =>
        _app?.Services ?? throw new InvalidOperationException("App not initialized.");

    public void ClearOutputHelper() => OutputHelper = null;

    public void SetOutputHelper(ITestOutputHelper value) => OutputHelper = value;

    public async ValueTask InitializeAsync()
    {
        _dbContainer = new PostgreSqlBuilder().Build();
        await _dbContainer.StartAsync();

        var builder = new DbContextOptionsBuilder<WebAppDbContext>();
        var connectionString = $"{_dbContainer.GetConnectionString()};Include Error Detail=true";

        builder.UseNpgsql(connectionString);
        await using var dbContext = new WebAppDbContext(builder.Options);
        await dbContext.Database.EnsureCreatedAsync();

        var factory = new ApplicationFactory<Program>();

        _app = factory
            .ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders().AddXUnit(this))
            .UseHostConfiguration(config =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        { "IsAutomatedTest", "true" }, // Turn off external services in tests
                        { "ConnectionStrings:appdb", connectionString },
                    }
                );
            })
            .UseAppConfiguration(options =>
                options.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        { "IsAutomatedTest", "true" }, // Turn off external services in tests
                        { "ConnectionStrings:appdb", connectionString },
                    }
                )
            );
    }

    public async ValueTask DisposeAsync()
    {
        if (_app == null)
        {
            return;
        }

        await _app.DisposeAsync();
        _app = null;
    }
}
