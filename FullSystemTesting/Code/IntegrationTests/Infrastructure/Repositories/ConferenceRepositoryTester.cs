using CodeCampServerLite.Core.Domain.Model;
using CodeCampServerLite.Infrastructure.DataAccess.Repositories;
using Should;
using NUnit.Framework;

namespace CodeCampServerLite.IntegrationTests.Infrastructure.Repositories
{
    [TestFixture]
    public class ConferenceRepositoryTester : IntegrationTestBase
    {
        [Test]
        public void Should_get_events_by_name()
        {
            var conference = new Conference("Foo");
            
            SaveEntities(conference);

            var repos = new ConferenceRepository(SessionSource.CreateSession());

            var loaded = repos.GetByName("Foo");

            loaded.ShouldEqual(conference);
        }
    }
}