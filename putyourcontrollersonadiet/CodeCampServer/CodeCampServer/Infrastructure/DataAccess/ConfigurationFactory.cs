using System.Collections.Generic;
using System.Data;
using FluentNHibernate.Cfg;
using CodeCampServerLite.Core.Domain.Model;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace CodeCampServerLite.Infrastructure.DataAccess
{
    using Core.Domain.Model;

    public class ConfigurationFactory
    {
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

        public void BuildSchema(ISession session, Configuration configuration)
        {
            IDbConnection connection = session.Connection;

            Dialect dialect = Dialect.GetDialect(configuration.Properties);
            string[] drops = configuration.GenerateDropSchemaScript(dialect);
            ExecuteScripts(drops, connection);

            string[] scripts = configuration.GenerateSchemaCreationScript(dialect);
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