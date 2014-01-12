using System.Collections.Generic;
using System.Data;
using CodeCampServerLite.Core.Domain.Model;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace CodeCampServerLite.Infrastructure.DataAccess
{
    using FluentNHibernate.Cfg.Db;
    using NHibernate.Bytecode;
    using NHibernate.Caches.SysCache;
    using NHibernate.Tool.hbm2ddl;

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
            return Fluently.Configure()
                .Cache(csb => csb.ProviderClass<SysCacheProvider>().UseQueryCache())
                .Database(() =>
                {
                    var config = MsSqlConfiguration.MsSql2008;
                    config.ConnectionString(cse => cse.FromConnectionStringWithKey("App"));
                    return config;
                })
                .ProxyFactoryFactory<DefaultProxyFactoryFactory>()
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

        public ISessionFactory GetSessionFactory()
        {
            return _sessionFactory;
        }

        public Configuration GetConfiguration()
        {
            return _configuration;
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