using StructureMap.Configuration.DSL;

namespace CodeCampServerLite.Infrastructure.IoC
{
    using FluentValidation;
    using MediatR;
    using StructureMap.Graph;

    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            Scan(cfg =>
            {
                cfg.TheCallingAssembly();
                cfg.Assembly("CodeCampServerLite.UI");

                cfg.AssemblyContainingType<IMediator>();

                cfg.WithDefaultConventions();
                cfg.AddAllTypesOf<IStartupTask>();

                cfg.AddAllTypesOf(typeof(IRequestHandler<,>));
                cfg.AddAllTypesOf(typeof(IValidator<>));
            });
        }
    }
}