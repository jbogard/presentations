var builder = DistributedApplication.CreateBuilder(args);

var rmqPassword = builder.AddParameter("messaging-password");
var dbPassword = builder.AddParameter("db-password");

var broker = builder.AddRabbitMQ(name: "broker", password: rmqPassword, port: 5672)
    .WithDataVolume()
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEndpoint("management", e => e.Port = 15672);

var cosmos = builder.AddAzureCosmosDB("cosmos-db")
    .RunAsEmulator(emulator =>
    {
        emulator.WithLifetime(ContainerLifetime.Persistent);
        emulator.WithGatewayPort(8081);
        emulator.WithImage("cosmosdb/linux/azure-cosmos-emulator", tag: "vnext-preview");
    });

var sql = builder.AddSqlServer("sql", password: dbPassword)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("AdventureWorks2016");

var web = builder
    .AddProject<Projects.AdventureWorksDistributed_UI>("web")
    .WithReference(broker)
    .WithReference(sql)
    .WaitFor(broker)
    .WaitFor(sql);


var fulfillment = builder
    .AddProject<Projects.AdventureWorksDistributed_Fulfillment_Endpoint>("fulfillment")
    .WithReference(broker)
    .WithReference(sql)
    .WaitFor(broker)
    .WaitFor(sql);

var inventory = builder
    .AddProject<Projects.AdventureWorksDistributed_Inventory_Endpoint>("inventory")
    .WithReference(broker)
    .WithReference(cosmos)
    .WaitFor(broker)
    .WaitFor(cosmos);

var orders = builder
    .AddProject<Projects.AdventureWorksDistributed_Orders_Api>("orders")
    .WithReference(broker)
    .WithReference(cosmos)
    .WaitFor(broker)
    .WaitFor(cosmos);

builder.Build().Run();