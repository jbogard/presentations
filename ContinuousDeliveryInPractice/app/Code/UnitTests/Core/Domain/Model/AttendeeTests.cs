namespace CodeCampServerLite.UnitTests.Core.Domain.Model
{
    using System.Linq;
    using CodeCampServerLite.Core.Domain.Model;
    using Should;
    using Xunit;

    public class AttendeeTests
    {
        [Fact]
        public void Should_construct_attendee_with_first_and_last_name()
        {
            var attendee = new Attendee("Joe", "Schmoe");

            attendee.FirstName.ShouldEqual("Joe");
            attendee.LastName.ShouldEqual("Schmoe");
        }


        [Fact]
        public void Should_create_attendee_association_when_registering_for_an_event()
        {
            var conference = new Conference("Austin Code Camp");
            var attendee = new Attendee("Joe", "Schmoe");

            attendee.RegisterFor(conference);

            attendee.Conference.ShouldEqual(conference);
            conference.GetAttendees().Any(a => a == attendee).ShouldBeTrue();
        }
    }
}