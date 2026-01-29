using Data;
using FastEndpoints;
using FastEndpoints.Security;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Scalar.AspNetCore;
using WebApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

#region Example 2
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
#endregion

#region Example 2
builder.Services.AddFastEndpoints();
#endregion

#region Example 3
builder.Services
    .AddAuthenticationJwtBearer(s =>
    {
        s.SigningKey = "A super super super secret token signing key";
    })
    .AddAuthorization();
#endregion

#region Example 4
builder.Services.AddHttpClient<IWeatherForecastClient, WeatherForecastClient>(
    static client => client.BaseAddress = new("https+http://externalapi"));
#endregion

#region Example 5
builder.Services.AddRazorPages();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});
#endregion

#region Example 6
var isAutomatedTest = builder.Configuration.GetValue<bool?>("IsAutomatedTest")
                      ?? false;

if (!isAutomatedTest)
{
    builder.Services.AddHangfire(config =>
        config.UsePostgreSqlStorage(c =>
        {
            c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("appdb"));
        }));

    builder.Services.AddHangfireServer();
}
#endregion

#region Example 7
if (!isAutomatedTest)
{
    //builder.UseNServiceBusWithConfiguration("webapp", "appdb");
}
#endregion

var app = builder.Build();

app.MapDefaultEndpoints();

#region Example 2
app.UseFastEndpoints();
#endregion

#region Example 5
app.MapStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    #region Example 6
    app.UseHangfireDashboard();
    #endregion
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

#region Example 2
SeedDatabase(app);
#endregion

#region Example 6
ConfigureHangfireJobs(builder, app);
#endregion
     
app.Run();

#region Example 2
static void SeedDatabase(WebApplication app)
{
    var isAutomatedTest = app.Configuration.GetValue<bool?>("IsAutomatedTest")
                          ?? false;
    
    if (isAutomatedTest)
        return;
    
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<WebAppDbContext>();
    var strategy = db.Database.CreateExecutionStrategy();

    strategy.Execute(() => { db.Database.EnsureCreated(); });
}
#endregion

#region Example 6
static void ConfigureHangfireJobs(IHostApplicationBuilder builder, IHost host)
{
    var isAutomatedTest = builder.Configuration.GetValue<bool?>("IsAutomatedTest")
        ?? false;

    if (isAutomatedTest) 
        return;
    
    var recurringJob = host.Services.GetRequiredService<IRecurringJobManager>();

    recurringJob.AddOrUpdate<TodoWreckerJob>("todowreckerjob", job => job.DoWork(CancellationToken.None),
        "*/30 * * * * *");
}
#endregion

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

#region Example 1
public partial class Program { }
#endregion