using StructureMap;

namespace CodeCampServerLite.Infrastructure.IoC
{
    public static class StructureMapConfiguration
    {
        public static void Initialize()
        {
            ObjectFactory.Initialize(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.TheCallingAssembly();
                    scanner.LookForRegistries();
                });
            });
        }

    }
}