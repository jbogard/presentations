//using System;
//using System.Net.Http;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.Extensions.DependencyInjection;
//using WorkerService;
//using Xunit;

//namespace IntegrationTests;

//public class SystemFixture : IDisposable
//{
//    public WebAppFactory WebAppHost { get; }

//    public WebApplicationFactory<Program> WorkerHost { get; }

//    public ChildWorkerServiceFactory ChildWorkerHost { get; }

//    public SystemFixture()
//    {
//        ChildWorkerHost = new ChildWorkerServiceFactory();
//        WorkerHost = new WorkerServiceFactory()
//            .WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
//            {
//                services.AddScoped<Func<HttpClient>>(_ => () => WebAppHost.CreateClient());
//            }));
//        WebAppHost = new WebAppFactory();
//    }

//    public void Start()
//    {
//        WorkerHost.CreateClient();
//        WebAppHost.CreateClient();
//        ChildWorkerHost.CreateClient();
//    }

//    public void Dispose()
//    {
//        WebAppHost?.Dispose();
//        WorkerHost?.Dispose();
//        ChildWorkerHost?.Dispose();
//    }
//}

//[CollectionDefinition(nameof(SystemCollection), DisableParallelization = true)]
//public class SystemCollection : ICollectionFixture<SystemFixture> { }