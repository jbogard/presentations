namespace CodeCampServerLite.Infrastructure.DataAccess
{
    public class NHibernateConfiguration : IStartupTask
    {
        private readonly ISessionSource _sessionSource;

        public NHibernateConfiguration(ISessionSource sessionSource)
        {
            _sessionSource = sessionSource;
        }

        public void Execute()
        {
            _sessionSource.BuildSchema();
        }
    }
}