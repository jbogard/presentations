using AdventureWorksDistributed.Core.Infrastructure;
using AdventureWorksDistributed.Orders.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using NServiceBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var CosmosUrl = "https://localhost:8081/";
var CosmosKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

var client = new DocumentClient(new Uri(CosmosUrl), CosmosKey, new JsonSerializerSettings
{
    TypeNameHandling = TypeNameHandling.Auto
});

builder.Services.AddSingleton(client);

builder.Services.AddTransient(typeof(IDocumentDbRepository<>), typeof(DocumentDbRepository<>));


builder.Host.UseNServiceBus(context =>
{
    var endpointConfiguration = new EndpointConfiguration("AdventureWorksDistributed.Orders");

    endpointConfiguration.UseTransport<RabbitMQTransport>()
        .ConnectionString("host=localhost")
        .UseConventionalRoutingTopology(QueueType.Classic);

    var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>()
        .CosmosClient(new CosmosClient(context.Configuration.GetConnectionString("Cosmos")))
        .DatabaseName("AdventureWorksDistributed")
        .DefaultContainer("Orders", "/OrderId");

    var transactionInformation = persistence.TransactionInformation();
    transactionInformation
        .ExtractPartitionKeyFromMessage<IProvideOrderId>(
            provideOrderId => new PartitionKey(provideOrderId.OrderId.ToString()));

    endpointConfiguration.EnableOutbox();

    endpointConfiguration.EnableInstallers();

    return endpointConfiguration;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
