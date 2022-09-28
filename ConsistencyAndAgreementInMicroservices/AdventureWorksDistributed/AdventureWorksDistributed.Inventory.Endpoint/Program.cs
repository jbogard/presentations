using AdventureWorksDistributed.Core.Infrastructure;
using AdventureWorksDistributed.Inventory.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using NServiceBus;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var CosmosUrl = "https://localhost:8081/";
        var CosmosKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        var client = new DocumentClient(new Uri(CosmosUrl), CosmosKey, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });

        services.AddSingleton(client);

        services.AddTransient(typeof(IDocumentDbRepository<>), typeof(DocumentDbRepository<>));
    })
    .UseNServiceBus(context =>
    {
        var endpointConfiguration = new EndpointConfiguration("AdventureWorksDistributed.Inventory");

        endpointConfiguration.UseTransport<RabbitMQTransport>()
            .ConnectionString("host=localhost")
            .UseConventionalRoutingTopology(QueueType.Classic);

        var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>()
            .CosmosClient(new CosmosClient(context.Configuration.GetConnectionString("Cosmos")))
            .DatabaseName("AdventureWorksDistributed")
            .DefaultContainer("Inventory", "/ProductId");

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation
            .ExtractPartitionKeyFromMessage<IProvideProductId>(
                provideProductId => new PartitionKey(provideProductId.ProductId.ToString()));

        //endpointConfiguration.EnableOutbox();

        endpointConfiguration.EnableInstallers();

        return endpointConfiguration;
    })
    .Build();

await host.RunAsync();
