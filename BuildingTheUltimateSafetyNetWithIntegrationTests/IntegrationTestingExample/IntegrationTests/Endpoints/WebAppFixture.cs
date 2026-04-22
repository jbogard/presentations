#region Example 2
using Bogus.DataSets;
using Data;
using FastEndpoints.Testing;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NServiceBus.Testing;
using Testcontainers.PostgreSql;
using WebApp.Infrastructure;

namespace IntegrationTests.Endpoints;

public class WebAppFixture : AppFixture<Program>, ITestOutputHelperAccessor
{
    private PostgreSqlContainer _dbContainer = null!;
    public HttpClient AuthClient { get; private set; } = null!;

    public ITestOutputHelper? OutputHelper { get; set; }

    public void ClearOutputHelper() => OutputHelper = null;

    public void SetOutputHelper(ITestOutputHelper value) => OutputHelper = value;

    protected override async ValueTask PreSetupAsync()
    {
        _dbContainer = new PostgreSqlBuilder().Build();
        await _dbContainer.StartAsync();

        var builder = new DbContextOptionsBuilder<WebAppDbContext>();
        builder.UseNpgsql(_dbContainer.GetConnectionString());
        
        await using var dbContext = new WebAppDbContext(builder.Options);
        await dbContext.Database.EnsureCreatedAsync();
    }

    protected override IHost ConfigureAppHost(IHostBuilder builder)
    {
        builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders().AddXUnit(this));

        var connectionString = $"{_dbContainer.GetConnectionString()};Include Error Detail=true";
        builder.ConfigureHostConfiguration(options =>
            options
                .AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        { "ConnectionStrings:appdb", connectionString },
                        { "IsAutomatedTest", "true" }, // Turn off external services in tests
                    }
                )
        );

        builder.ConfigureAppConfiguration(options =>
            options
                .AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        { "ConnectionStrings:appdb", connectionString },
                    }
                )
        );

        return base.ConfigureAppHost(builder);
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        #region Example 3
        services.Configure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            options =>
            {
                options.Configuration = new OpenIdConnectConfiguration
                {
                    Issuer = JwtProvider.Issuer,
                };
                options.TokenValidationParameters.ValidIssuer = JwtProvider.Issuer;
                options.TokenValidationParameters.ValidAudience = JwtProvider.Issuer;
                options.Configuration.SigningKeys.Add(JwtProvider.SecurityKey);
            }
        );
        #endregion

        #region Example 4
        services.AddSingleton<IWeatherForecastClient, FakeWeatherForecastClient>();
        #endregion
        
        #region Example 7
        services.AddSingleton<IMessageSession, TestableMessageSession>();
        #endregion
        
        base.ConfigureServices(services);
    }
 
    #region Example 3
    protected override ValueTask SetupAsync()
    {
        AuthClient = Client.WithJwtBearerToken();
    
        return base.SetupAsync();
    }
    #endregion
   
    #region Example 4
    public class FakeWeatherForecastClient : IWeatherForecastClient
    {
        public static readonly WeatherForecast WeatherForecast = new(DateOnly.FromDateTime(DateTime.UtcNow), 34, "Sunny");
    
        public Task<IEnumerable<WeatherForecast>?> GetAsync()
        {
            return Task.FromResult<IEnumerable<WeatherForecast>?>([
                WeatherForecast
            ]);
        }
    }
    #endregion
}

#endregion