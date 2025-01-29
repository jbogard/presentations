using AdventureWorksDistributed.Inventory.Contracts;
using AdventureWorksDistributed.Orders.Contracts;
using AdventureWorksDistributed.PartitionKeys;
using Microsoft.Azure.Cosmos;
using NServiceBus;

IHost host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(context =>
    {
        var endpointConfiguration = new EndpointConfiguration("AdventureWorksDistributed.Fulfillment");

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>()
            .ConnectionString("host=localhost")
            .UseConventionalRoutingTopology(QueueType.Classic);

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var routing = transport.Routing();

        routing.RouteToEndpoint(
            typeof(StockRequest).Assembly,
            "AdventureWorksDistributed.Inventory");
        routing.RouteToEndpoint(
            typeof(OrderCreated).Assembly,
            "AdventureWorksDistributed.Orders");

        var persistence = endpointConfiguration.UsePersistence<CosmosPersistence>()
            .CosmosClient(new CosmosClient(context.Configuration.GetConnectionString("Cosmos")))
            .DatabaseName("AdventureWorksDistributed")
            .DefaultContainer("Fulfillment", "/OrderId");

        var transactionInformation = persistence.TransactionInformation();
        transactionInformation
            .ExtractPartitionKeyFromMessage<IProvideOrderId>(
                provideProductId => new PartitionKey(provideProductId.OrderId.ToString()));

        endpointConfiguration.EnableOutbox();

        endpointConfiguration.EnableInstallers();

        return endpointConfiguration;
    })
    .Build();

await host.RunAsync();
