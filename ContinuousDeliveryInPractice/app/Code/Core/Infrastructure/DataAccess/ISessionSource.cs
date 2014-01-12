using NHibernate;

namespace CodeCampServerLite.Infrastructure.DataAccess
{
    using NHibernate.Cfg;

    public interface ISessionSource
    {
        ISession CreateSession();
        ISessionFactory GetSessionFactory();
        Configuration GetConfiguration();
        void BuildSchema();
    }
}