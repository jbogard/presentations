using CodeCampServerLite.Infrastructure.DataAccess;
using NHibernate;
using StructureMap.Configuration.DSL;

namespace CodeCampServerLite.Infrastructure.IoC
{
    public class NHibernateRegistry : Registry
    {
        public NHibernateRegistry()
        {
            ForSingletonOf<ISessionSource>().Use<NHibernateSessionSource>();

            For<ISession>().Use(c =>
            {
                var transaction = (NHibernateTransactionBoundary)c.GetInstance<ITransactionBoundary>();
                return transaction.CurrentSession;
            });

            For<ITransactionBoundary>().HybridHttpOrThreadLocalScoped().Use<NHibernateTransactionBoundary>();
        }
    }
}