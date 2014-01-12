namespace CodeCampServerLite.UnitTests.Core.Domain.Model
{
    using System.Linq;
    using CodeCampServerLite.Core.Domain.Model;
    using Should;
    using Xunit;

    public class ConferenceTests
    {
        [Fact]
        public void Should_construct_with_name()
        {
            var conference = new Conference("Something");

            conference.Name.ShouldEqual("Something");
        }

        [Fact]
        public void Should_build_association_with_session_when_adding_session()
        {
            var conference = new Conference("Something");
            var session = new Session("Foo", "Bar", new Speaker("Joe", "Schmoe"));

            conference.AddSession(session);

            session.Conference.ShouldEqual(conference);
            conference.GetSessions().Any(s => s == session).ShouldBeTrue();
        }

        [Fact]
        public void Should_be_able_to_change_the_name()
        {
            var conference = new Conference("Foo");

            conference.ChangeName("Bar");

            conference.Name.ShouldEqual("Bar");
        }

        [Fact]
        public void Should_update_the_session_count_when_adding_a_session()
        {
            var conference = new Conference("Something");
            var session = new Session("Foo", "Bar", new Speaker("Joe", "Schmoe"));

            conference.AddSession(session);

            conference.SessionCount.ShouldEqual(1);
        }

        [Fact]
        public void Should_update_the_attendee_count_when_adding_an_attendee()
        {
            var conference = new Conference("Something");

            var attendee = new Attendee("Foo", "Bar");
            attendee.RegisterFor(conference);

            conference.AttendeeCount.ShouldEqual(1);
        }
    }
}