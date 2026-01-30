using System.Diagnostics;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

#region Add Custom Containers
// builder.AddContainer("zipkin", "openzipkin/zipkin")
//     .WithHttpEndpoint(9411, 9411);
#endregion

#region Add Message Broker
// var rmqPassword = builder.AddParameter("messaging-password");
//
// var broker = builder.AddRabbitMQ(name: "broker", password: rmqPassword, port: 5672)
//     .WithDataVolume()
//     .WithManagementPlugin()
//     .WithLifetime(ContainerLifetime.Persistent)
//     .WithEndpoint("management", e => e.Port = 15672);
#endregion

#region Add MongoDB
var mongo = builder.AddMongoDB("mongo")
    .WithLifetime(ContainerLifetime.Persistent);
#endregion

#region Add Database

// var dbPassword = builder.AddParameter("db-password");
//
// var sql = builder.AddSqlServer("sql", password: dbPassword)
//     .WithDataVolume()
//     .WithLifetime(ContainerLifetime.Persistent)
//     .AddDatabase("sqldata")
//     
//     #region Add Custom Migration Command
//     // .WithCommand(
//     //     name: $"migrate-sql-db",
//     //     displayName: "Migrate",
//     //     executeCommand: async cmd =>
//     //     {
//     //         var process = new Process();
//     //         process.StartInfo.WorkingDirectory = $"../WebApplication";
//     //         process.StartInfo.FileName = "dotnet";
//     //         process.StartInfo.Arguments = "ef database update";
//     //         process.StartInfo.UseShellExecute = true;
//     //
//     //         process.Start();
//     //
//     //         await process.WaitForExitAsync(cmd.CancellationToken);
//     //
//     //         return CommandResults.Success();
//     //     },
//     //     commandOptions: new CommandOptions { IconName = "ArrowSquareUpRight" }
//     // )
//     #endregion
//     ;
#endregion

#region Add External API
// var externalApi = builder
//     .AddProject<ExternalApi>("externalapi")
//     #region Add Custom Endpoint
//     // .WithUrlForEndpoint("http", url =>
//     // {
//     //     url.DisplayText = "Scalar (HTTP)";
//     //     url.Url = "/scalar";
//     // })
//     #endregion
//     ;
#endregion

var web = builder
    .AddProject<WebApplication>("web")
    #region Add Dependencies
    // .WithReference(externalApi)
    // .WithReference(broker)
    // .WithReference(sql)
    // .WaitFor(broker)
    // .WaitFor(sql)
    #endregion
    ;

#region Add Worker Services
// var childWorker = builder
//     .AddProject<ChildWorkerService>("childworker")
//     .WithReference(broker)
//     .WithReference(mongo)
//     .WaitFor(broker);
//
// var worker = builder
//     .AddProject<WorkerService>("worker")
//     .WithReference(broker)
//     .WithReference(childWorker)
//     .WithReference(web)
//     .WaitFor(broker);
#endregion

var application = builder.Build();

application.Run();

public partial class Program { }
