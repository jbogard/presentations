using FastEndpoints;
using FastEndpoints.Security;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Scalar.AspNetCore;
using WebApp.Data;
using WebApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<WebAppDbContext>(connectionName: "appdb", configureDbContextOptions: options =>
{
    options.UseSeeding((db, _) =>
    {
            var todoItems = db.Set<TodoItem>();

            if (!todoItems.Any())
            {
                todoItems.Add(new TodoItem { Name = "Todo item 1" });
                todoItems.Add(new TodoItem { Name = "Todo item 2" });
                todoItems.Add(new TodoItem { Name = "Todo item 3" });
                todoItems.Add(new TodoItem { Name = "Todo item 4" });
                todoItems.Add(new TodoItem { Name = "Todo item 5" });
            }

            db.SaveChanges();
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddRazorPages();

builder.Services
    .AddAuthenticationJwtBearer(s =>
    {
        s.SigningKey = "A super super super secret token signing key";
    })
    .AddAuthorization()
    .AddFastEndpoints();

builder.Services.AddHttpClient<IWeatherForecastClient, WeatherForecastClient>(
    static client => client.BaseAddress = new("https+http://externalapi"));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(c =>
    {
        c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("appdb"));
    }));

builder.Services.AddHangfireServer();

UseNServiceBusWithConfiguration(builder, "webapp", "appdb");

var app = builder.Build();

app.UseFastEndpoints();

app.MapDefaultEndpoints();

app.MapStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseHangfireDashboard();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

SeedDatabase(app);
ConfigureHangfireJobs(builder, app);
    
app.Run();


static void SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<WebAppDbContext>();
    var strategy = db.Database.CreateExecutionStrategy();

    strategy.Execute(() => { db.Database.EnsureCreated(); });
}

static void ConfigureHangfireJobs(IHostApplicationBuilder builder, IHost host)
{
    var enableHangfire = builder.Configuration.GetValue<bool>("EnableHangfire");

    if (enableHangfire)
    {
        var recurringJob = host.Services.GetRequiredService<IRecurringJobManager>();

        recurringJob.AddOrUpdate<TodoWreckerJob>("todowreckerjob", job => job.DoWork(CancellationToken.None),
            "*/30 * * * * *");
    }
}

static void UseNServiceBusWithConfiguration(
    IHostApplicationBuilder builder, 
    string endpointName,
    string postgresConnectionStringName)
    {
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        // RabbitMQ Transport: https://docs.particular.net/transports/rabbitmq/
        endpointConfiguration.UseTransport<LearningTransport>();
        
        var dbConnectionString = builder.Configuration.GetConnectionString(postgresConnectionStringName);
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
        dialect.JsonBParameterModifier(
            modifier: parameter =>
            {
                var npgsqlParameter = (NpgsqlParameter)parameter;
                npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
            });
        
        persistence.ConnectionBuilder(() => new NpgsqlConnection(dbConnectionString));
        
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.EnableInstallers();
        
        endpointConfiguration.EnableOpenTelemetry();
        
        builder.UseNServiceBus(endpointConfiguration);
    }


public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
