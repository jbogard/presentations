using System.Linq;
using CodeCampServerLite.Core.Domain.Model;
using Should;
using NUnit.Framework;

namespace CodeCampServerLite.UnitTests.Core.Domain.Model
{
	[TestFixture]
	public class AttendeeTests
	{
		[Test]
		public void Should_construct_attendee_with_first_and_last_name()
		{
			var attendee = new Attendee("Joe", "Schmoe");

			attendee.FirstName.ShouldEqual("Joe");
			attendee.LastName.ShouldEqual("Schmoe");
		}


		[Test]
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