namespace ContosoUniversity
{
    using AutoMapper;
    using DAL;
    using DependencyResolution;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using StructureMap.Pipeline;

    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                    scan.LookForRegistries();
                    scan.AssemblyContainingType<DefaultRegistry>();
                    scan.AddAllTypesOf<Profile>();
                });
            For<SchoolContext>().Use<SchoolContext>().LifecycleIs<TransientLifecycle>();
            //For<IExample>().Use<Example>();
        }
    }
}