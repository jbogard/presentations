using StructureMap.Configuration.DSL;

namespace CodeCampServerLite.Infrastructure.IoC
{
    using StructureMap.Graph;

    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            Scan(cfg =>
            {
                cfg.TheCallingAssembly();
                cfg.WithDefaultConventions();
                cfg.AddAllTypesOf<IStartupTask>();
            });
        }
    }
}