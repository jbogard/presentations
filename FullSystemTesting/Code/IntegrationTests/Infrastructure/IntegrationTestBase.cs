using CodeCampServerLite.Core.Domain.Model;
using CodeCampServerLite.Infrastructure.DataAccess;
using NHibernate;
using NUnit.Framework;
using StructureMap;

namespace CodeCampServerLite.IntegrationTests.Infrastructure
{
    [TestFixture]
    public abstract class IntegrationTestBase
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            BootStrapper.Bootstrap();

            SessionSource = ObjectFactory.GetInstance<ISessionSource>();
            SessionSource.BuildSchema();
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
}