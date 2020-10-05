using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Divergent.ITOps.Interfaces;
using ITOps.EndpointConfig;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Divergent.ITOps
{
    public class Program
    {
        public static string EndpointName => "Divergent.ITOps";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((builder, services) =>
                {
                    var assemblies = ReflectionHelper.GetAssemblies("..\\..\\..\\Providers", ".Data.dll");
                    services.Scan(s =>
                    {
                        s.FromAssemblies(assemblies)
                            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Provider")))
                            .AsImplementedInterfaces()
                            .WithTransientLifetime();
                    });

                    var serviceRegistrars = assemblies
                        .SelectMany(a => a.GetTypes())
                        .Where(t => typeof(IRegisterServices).IsAssignableFrom(t))
                        .Select(Activator.CreateInstance)
                        .Cast<IRegisterServices>()
                        .ToList();

                    foreach (var serviceRegistrar in serviceRegistrars)
                    {
                        serviceRegistrar.Register(builder, services);
                    }

                    //services.AddDbContext<FinanceContext>(options =>
                    //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
                })
                .UseNServiceBus(context =>
                {
                    var endpoint = new EndpointConfiguration(EndpointName);
                    endpoint.Configure();

                    return endpoint;
                });
    }
}
