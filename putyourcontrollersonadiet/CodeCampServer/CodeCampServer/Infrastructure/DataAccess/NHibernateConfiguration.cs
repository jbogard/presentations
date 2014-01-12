namespace CodeCampServerLite.Infrastructure.DataAccess
{
    using NHibernate;
    using NHibernate.Cfg;

    public class NHibernateConfiguration : IStartupTask
    {
        private readonly ISession _session;
        private readonly Configuration _configuration;

        public NHibernateConfiguration(ISession session, Configuration configuration)
        {
            _session = session;
            _configuration = configuration;
        }

        public void Execute()
        {
            var factory = new ConfigurationFactory();
            factory.BuildSchema(_session, _configuration);
        }
    }
}