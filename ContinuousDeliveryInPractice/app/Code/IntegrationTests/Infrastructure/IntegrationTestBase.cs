namespace CodeCampServerLite.IntegrationTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using CodeCampServerLite.Infrastructure.DataAccess;
    using Core.Domain.Model;
    using NHibernate;
    using NHibernate.Transform;
    using StructureMap;

    public abstract class IntegrationTestBase
    {
        protected IntegrationTestBase()
        {
            BootStrapper.Bootstrap();

            var deleter = new DatabaseDeleter(ObjectFactory.GetInstance<ISessionFactory>());
            deleter.DeleteAllData();
            SessionSource = ObjectFactory.GetInstance<ISessionSource>();
        }

        protected ISessionSource SessionSource { get; private set; }

        protected void SaveEntities(params Entity[] entities)
        {
            using (var session = SessionSource.CreateSession())
            using (var tx = session.BeginTransaction())
            {
                entities.Each(session.SaveOrUpdate);

                tx.Commit();
            }
        }
    }

    public class DatabaseDeleter
    {
        private readonly ISessionFactory _configuration;
        private static readonly string[] _ignoredTables = new[] { "sysdiagrams", "Version", "ScriptsRun", "ScriptsRunErrors" };
        private static string[] _tablesToDelete;
        private static string _deleteSql;
        private static object _lockObj = new object();
        private static bool _initialized;

        public DatabaseDeleter(ISessionFactory sessionSource)
        {
            _configuration = sessionSource;

            BuildDeleteTables();
        }

        private class Relationship
        {
            public string PrimaryKeyTable { get; private set; }
            public string ForeignKeyTable { get; private set; }
        }

        public virtual void DeleteAllData()
        {
            ISession session = _configuration.OpenSession();

            using (IDbCommand command = session.Connection.CreateCommand())
            {
                command.CommandText = _deleteSql;
                command.ExecuteNonQuery();
            }
        }

        public static string[] GetTables()
        {
            return _tablesToDelete;
        }

        private void BuildDeleteTables()
        {
            if (!_initialized)
            {
                lock (_lockObj)
                {
                    if (!_initialized)
                    {
                        ISession session = _configuration.OpenSession();

                        var allTables = GetAllTables(session);

                        var allRelationships = GetRelationships(session);

                        _tablesToDelete = BuildTableList(allTables, allRelationships);

                        _deleteSql = BuildTableSql(_tablesToDelete);

                        _initialized = true;
                    }
                }
            }
        }

        private static string BuildTableSql(IEnumerable<string> tablesToDelete)
        {
            string completeQuery = "";
            foreach (var tableName in tablesToDelete)
            {
                completeQuery += String.Format("delete from [{0}];", tableName);
            }
            return completeQuery;
        }

        private static string[] BuildTableList(ICollection<string> allTables, ICollection<Relationship> allRelationships)
        {
            var tablesToDelete = new List<string>();

            while (allTables.Any())
            {
                var leafTables = allTables.Except(allRelationships.Select(rel => rel.PrimaryKeyTable)).ToArray();

                tablesToDelete.AddRange(leafTables);

                foreach (var leafTable in leafTables)
                {
                    allTables.Remove(leafTable);
                    var relToRemove = allRelationships.Where(rel => rel.ForeignKeyTable == leafTable).ToArray();
                    foreach (var rel in relToRemove)
                    {
                        allRelationships.Remove(rel);
                    }
                }
            }

            return tablesToDelete.ToArray();
        }

        private static IList<Relationship> GetRelationships(ISession session)
        {
            var otherquery = session.CreateSQLQuery(
            @"select
			so_pk.name as PrimaryKeyTable
		,   so_fk.name as ForeignKeyTable
		from
			sysforeignkeys sfk
				inner join sysobjects so_pk on sfk.rkeyid = so_pk.id
				inner join sysobjects so_fk on sfk.fkeyid = so_fk.id
		order by
			so_pk.name
		,   so_fk.name");

            return otherquery.SetResultTransformer(Transformers.AliasToBean<Relationship>()).List<Relationship>();
        }

        private static IList<string> GetAllTables(ISession session)
        {
            var query = session.CreateSQLQuery("select name from sys.tables");

            return query.List<string>().Except(_ignoredTables).ToList();
        }
    }
}