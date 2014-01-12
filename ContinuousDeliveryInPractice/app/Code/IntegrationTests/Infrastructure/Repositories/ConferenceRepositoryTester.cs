namespace CodeCampServerLite.IntegrationTests.Infrastructure.Repositories
{
    using CodeCampServerLite.Infrastructure.DataAccess.Repositories;
    using Core.Domain.Model;
    using Should;
    using Xunit;

    public class ConferenceRepositoryTester : IntegrationTestBase
    {
        [Fact]
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