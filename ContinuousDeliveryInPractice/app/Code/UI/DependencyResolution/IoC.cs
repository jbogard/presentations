using StructureMap;
namespace CodeCampServerLite.UI {
    using System;
    using Infrastructure.IoC;
    using Microsoft.Practices.ServiceLocation;
    using StructureMap.Graph;

    public static class IoC
    {
        private static Lazy<IContainer> init = new Lazy<IContainer>(() =>
        {
            var container = new Container(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<CoreRegistry>();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                });
                //                x.For<IExample>().Use<Example>();
            });
            
            var serviceLocator = new StructureMapServiceLocator(container);
            var serviceLocatorProvider = new ServiceLocatorProvider(() => serviceLocator);
            container.Configure(cfg => cfg.For<ServiceLocatorProvider>().Use(serviceLocatorProvider));

            return container;
        });

        public static IContainer Container 
        {
            get { return init.Value; }
        }
    }
}