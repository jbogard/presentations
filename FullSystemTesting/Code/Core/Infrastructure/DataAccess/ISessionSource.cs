using NHibernate;

namespace CodeCampServerLite.Infrastructure.DataAccess
{
    public interface ISessionSource
    {
        ISession CreateSession();
        void BuildSchema();
    }
}