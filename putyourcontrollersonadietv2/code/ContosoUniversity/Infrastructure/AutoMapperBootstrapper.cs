namespace ContosoUniversity.Infrastructure
{
    using AutoMapper;
    using StructureMap;

    public class AutoMapperBootstrapper
    {
        private static bool _initialized;
        private static readonly object Lock = new object();

        public static void Initialize(IContainer container)
        {
            lock (Lock)
            {
                if (_initialized) return;
                InitializeInternal(container);
                _initialized = true;
            }
        }

        private static void InitializeInternal(IContainer container)
        {
            Mapper.Initialize(cfg =>
            {
                foreach (var profile in container.GetAllInstances<Profile>())
                {
                    cfg.AddProfile(profile);
                }

                cfg.ConstructServicesUsing(container.GetInstance);
            });
        }
    }
}