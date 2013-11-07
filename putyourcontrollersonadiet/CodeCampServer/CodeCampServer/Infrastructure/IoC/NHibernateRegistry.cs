using CodeCampServerLite.Infrastructure.DataAccess;
using NHibernate;
using StructureMap.Configuration.DSL;

namespace CodeCampServerLite.Infrastructure.IoC
{
    using DataAccess;
    using NHibernate.Cfg;

    public class NHibernateRegistry : Registry
    {
        public NHibernateRegistry()
        {
            For<Configuration>().Singleton().Use(c => new ConfigurationFactory().AssembleConfiguration());
            For<ISessionFactory>().Singleton().Use(c => c.GetInstance<Configuration>().BuildSessionFactory());

            For<ISession>().HybridHttpOrThreadLocalScoped().Use(c =>
            {
                var sessionFactory = c.GetInstance<ISessionFactory>();
                return sessionFactory.OpenSession();
            });
        }
    }
}