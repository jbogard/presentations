namespace CodeCampServerLite.IntegrationTests.Infrastructure.Maps
{
    using System.Linq;
    using Core.Domain.Model;
    using Should;
    using Xunit;

    public class SessionMapTester : IntegrationTestBase
    {
        [Fact]
        public void Should_map_session_fields()
        {
            var speaker = new Speaker("Joe", "Schmoe");
            var session = new Session("Foo", new string('a', 5000), speaker);

            SaveEntities(session);

            var loaded = SessionSource.CreateSession().Load<Session>(session.Id);

            loaded.Title.ShouldEqual(session.Title);
            loaded.Abstract.ShouldEqual(session.Abstract);
        }

        [Fact]
        public void Should_cascade_to_speaker()
        {
            var speaker = new Speaker("Joe", "Schmoe");
            var session = new Session("Foo", "Bar", speaker);

            SaveEntities(session);

            var loaded = SessionSource.CreateSession().Load<Session>(session.Id);

            loaded.Speaker.ShouldEqual(speaker);

            loaded.Speaker.GetSessions().Count().ShouldEqual(1);
        }
    }
}