namespace CodeCampServerLite.IntegrationTests.Infrastructure.Maps
{
    using Core.Domain.Model;
    using Should;
    using Xunit;

    public class AttendeeMapTester : IntegrationTestBase
    {
        [Fact]
        public void Should_map_attendee_fields()
        {
            var attendee = new Attendee("Joe", "Schmoe")
            {
                Email = "foo@foo.com",
                State = "TX"
            };

            SaveEntities(attendee);

            var loaded = SessionSource.CreateSession().Load<Attendee>(attendee.Id);

            loaded.FirstName.ShouldEqual(attendee.FirstName);
            loaded.LastName.ShouldEqual(attendee.LastName);
            loaded.Email.ShouldEqual(attendee.Email);
            loaded.State.ShouldEqual(attendee.State);
        }
    }
}