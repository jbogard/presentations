using CodeCampServerLite.Infrastructure.DataAccess;
using NHibernate;
using StructureMap.Configuration.DSL;

namespace CodeCampServerLite.Infrastructure.IoC
{
    using NHibernate.Cfg;

    public class NHibernateRegistry : Registry
    {
        public NHibernateRegistry()
        {
            ForSingletonOf<ISessionSource>().Use<NHibernateSessionSource>();
            ForSingletonOf<ISessionFactory>().Use(ctx => ctx.GetInstance<ISessionSource>().GetSessionFactory());
            ForSingletonOf<Configuration>().Use(ctx => ctx.GetInstance<ISessionSource>().GetConfiguration());

            For<ISession>().Use(c =>
            {
                var transaction = (NHibernateTransactionBoundary)c.GetInstance<ITransactionBoundary>();
                return transaction.CurrentSession;
            });

            For<ITransactionBoundary>().HybridHttpOrThreadLocalScoped().Use<NHibernateTransactionBoundary>();
        }
    }
}