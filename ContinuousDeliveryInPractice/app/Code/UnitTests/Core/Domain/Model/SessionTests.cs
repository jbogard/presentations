namespace CodeCampServerLite.UnitTests.Core.Domain.Model
{
    using System.Linq;
    using CodeCampServerLite.Core.Domain.Model;
    using Should;
    using Xunit;

    public class SessionTests
    {
        [Fact]
        public void Should_construct_session_with_title_abstract_and_speaker()
        {
            var title = "Some Title";
            var @abstract = "A bunch of stuff";
            var speaker = new Speaker("Joe", "Schmoe");

            var session = new Session(title, @abstract, speaker);

            session.Title.ShouldEqual(title);
            session.Abstract.ShouldEqual(@abstract);
            session.Speaker.ShouldEqual(speaker);
            speaker.GetSessions().Any(s => s == session).ShouldBeTrue();
        }
    }
}