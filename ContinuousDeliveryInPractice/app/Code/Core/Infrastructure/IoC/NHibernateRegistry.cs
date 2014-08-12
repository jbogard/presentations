using CodeCampServerLite.Infrastructure.DataAccess;
using NHibernate;
using StructureMap.Configuration.DSL;

namespace CodeCampServerLite.Infrastructure.IoC
{
    using NHibernate.Cfg;
    using StructureMap.Web;

    public class NHibernateRegistry : Registry
    {
        public NHibernateRegistry()
        {
            ForSingletonOf<ISessionSource>().Use<NHibernateSessionSource>();
            ForSingletonOf<ISessionFactory>().Use(ctx => ctx.GetInstance<ISessionSource>().GetSessionFactory());
            ForSingletonOf<Configuration>().Use(ctx => ctx.GetInstance<ISessionSource>().GetConfiguration());

            For<ISession>().Use(c =>
                ((NHibernateTransactionBoundary)c.GetInstance<ITransactionBoundary>()).CurrentSession
            );

            For<ITransactionBoundary>()
                .HybridHttpOrThreadLocalScoped()
                .Use<NHibernateTransactionBoundary>()
                ;
        }
    }
}