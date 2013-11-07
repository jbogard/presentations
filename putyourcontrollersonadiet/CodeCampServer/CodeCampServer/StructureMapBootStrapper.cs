namespace CodeCampServerLite
{
    using Infrastructure;
    using Infrastructure.IoC;
    using StructureMap;

    public static class StructureMapBootStrapper
    {
        private static readonly object _lock = new object();
        private static bool _initialized;

        public static void Bootstrap()
        {
            if (! _initialized)
            {
                lock (_lock)
                {
                    if (! _initialized)
                    {
                        _initialized = true;

                        StructureMapConfiguration.Initialize();

                        var startupTasks = ObjectFactory.GetAllInstances<IStartupTask>();

                        startupTasks.Each(task => task.Execute());
                    }
                }
            }
        }
    }
}