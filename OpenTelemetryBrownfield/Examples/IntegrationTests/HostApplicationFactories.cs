//using ChildWorkerService.Messages;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Mongo2Go;
//using MongoDB.Driver;
//using NServiceBus;
//using NServiceBus.Extensions.IntegrationTesting;
//using WorkerService.Messages;

//namespace IntegrationTests;

//public class WebAppFactory : WebApplicationFactory<WebApplication.Program>
//{
//    protected override IHostBuilder CreateHostBuilder()
//    {
//        return Host.CreateDefaultBuilder()
//            .UseNServiceBus(_ =>
//            {
//                var endpoint = new EndpointConfiguration(WebApplication.Program.EndpointName);

//                endpoint.AssemblyScanner().ExcludeAssemblies("ChildWorkerService.dll", "WorkerService.dll");

//                endpoint.ConfigureTestEndpoint(transport =>
//                {
//                    var routing = transport.Routing();
//                    routing.RouteToEndpoint(typeof(SaySomething).Assembly, WorkerService.Program.EndpointName);
//                });

//                endpoint.UsePersistence<LearningPersistence>();
//                endpoint.UseSerialization<NewtonsoftJsonSerializer>();

//                return endpoint;
//            })
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.UseStartup<WebApplication.Startup>();
//            });
//    }
//}

//public class WorkerServiceFactory : WebApplicationFactory<WorkerService.Program>
//{
//    protected override IHostBuilder CreateHostBuilder()
//    {
//        return Host.CreateDefaultBuilder()
//            .UseNServiceBus(_ =>
//            {
//                var endpoint = new EndpointConfiguration(WorkerService.Program.EndpointName);

//                endpoint.AssemblyScanner().ExcludeAssemblies("WebApplication.dll", "ChildWorkerService.dll");

//                endpoint.ConfigureTestEndpoint(transport =>
//                {
//                    var routing = transport.Routing();
//                    routing.RouteToEndpoint(typeof(MakeItYell).Assembly, ChildWorkerService.Program.EndpointName);
//                });

//                endpoint.UsePersistence<LearningPersistence>();
//                endpoint.UseSerialization<NewtonsoftJsonSerializer>();

//                return endpoint;
//            })
//            .ConfigureWebHost(b => b.Configure(_ => {}));
//    }
//}

//public class ChildWorkerServiceFactory : WebApplicationFactory<ChildWorkerService.Program>
//{
//    protected override IHostBuilder CreateHostBuilder()
//    {
//        return Host.CreateDefaultBuilder()
//            .UseNServiceBus(_ =>
//            {
//                var endpoint = new EndpointConfiguration(ChildWorkerService.Program.EndpointName);

//                endpoint.ConfigureTestEndpoint();

//                endpoint.AssemblyScanner().ExcludeAssemblies("WebApplication.dll", "WorkerService.dll");

//                endpoint.UsePersistence<LearningPersistence>();
//                endpoint.UseSerialization<NewtonsoftJsonSerializer>();

//                return endpoint;
//            })
//            .ConfigureServices(services =>
//            {
//                var runner = MongoDbRunner.Start(singleNodeReplSet: true, singleNodeReplSetWaitTimeout: 20);

//                services.AddSingleton(runner);
//                var urlBuilder = new MongoUrlBuilder(runner.ConnectionString)
//                {
//                    DatabaseName = "dev"
//                };
//                var mongoUrl = urlBuilder.ToMongoUrl();
//                var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);
//                var mongoClient = new MongoClient(mongoClientSettings);
//                services.AddSingleton(mongoUrl);
//                services.AddSingleton(mongoClient);
//                services.AddTransient(provider => provider.GetService<MongoClient>().GetDatabase(provider.GetService<MongoUrl>().DatabaseName));
//                services.AddHostedService<ChildWorkerService.Mongo2GoService>();
//            })
//            .ConfigureWebHost(b => b.Configure(_ => { }));
//    }
//}