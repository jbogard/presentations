using Microsoft.EntityFrameworkCore;
using WebApplication;
using WorkerService.Messages;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder();

const string EndpointName = "NsbActivities.WebApplication";

var endpointConfiguration = new EndpointConfiguration(EndpointName);

var transport = new RabbitMQTransport(
    RoutingTopology.Conventional(QueueType.Quorum),
    builder.Configuration.GetConnectionString("broker")
);
var transportSettings = endpointConfiguration.UseTransport(transport);

transportSettings.RouteToEndpoint(typeof(SaySomething).Assembly, "NsbActivities.WorkerService");

endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

endpointConfiguration.EnableInstallers();

#region Enable Otel

//endpointConfiguration.EnableOpenTelemetry();

#endregion

endpointConfiguration.AuditProcessedMessagesTo("audit");

endpointConfiguration.ConnectToServicePlatformDefaults();

builder.UseNServiceBus(endpointConfiguration);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WeatherContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqldata")));

builder.Services.AddHostedService<DbInitializer>();

var app = builder.Build();

app.Logger.LogInformation(builder.Configuration.GetConnectionString("broker"));
app.Logger.LogInformation(builder.Configuration.GetConnectionString("sqldata"));

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
