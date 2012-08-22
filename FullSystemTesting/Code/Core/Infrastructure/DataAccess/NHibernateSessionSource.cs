using System.Collections.Generic;
using System.Data;
using CodeCampServerLite.Core.Domain.Model;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace CodeCampServerLite.Infrastructure.DataAccess
{
    public class NHibernateSessionSource : ISessionSource
    {
        private readonly object _factorySyncRoot = new object();
        private readonly ISessionFactory _sessionFactory;
        private readonly Configuration _configuration;

        public NHibernateSessionSource()
        {
            if (_sessionFactory != null) return;

            lock (_factorySyncRoot)
            {
                if (_sessionFactory != null) return;

                _configuration = AssembleConfiguration();
                _sessionFactory = _configuration.BuildSessionFactory();
            }
        }

        public Configuration AssembleConfiguration()
        {
            var configuration = new Configuration();

            configuration.Configure();

            return Fluently.Configure(configuration)
                .Mappings(cfg =>
                {
                    cfg.FluentMappings.AddFromAssemblyOf<Entity>();
                })
                .BuildConfiguration();
        }

        public ISession CreateSession()
        {
            return _sessionFactory.OpenSession();
        }

        public void BuildSchema()
        {
            ISession session = CreateSession();
            IDbConnection connection = session.Connection;

            Dialect dialect = Dialect.GetDialect(_configuration.Properties);
            string[] drops = _configuration.GenerateDropSchemaScript(dialect);
            ExecuteScripts(drops, connection);

            string[] scripts = _configuration.GenerateSchemaCreationScript(dialect);
            ExecuteScripts(scripts, connection);
        }

        private static void ExecuteScripts(IEnumerable<string> scripts, IDbConnection connection)
        {
            foreach (string script in scripts)
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = script;
                command.ExecuteNonQuery();
            }
        }

    }
}