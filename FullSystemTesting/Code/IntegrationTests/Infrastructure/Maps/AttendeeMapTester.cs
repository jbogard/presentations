using CodeCampServerLite.Core.Domain.Model;
using Should;
using NUnit.Framework;

namespace CodeCampServerLite.IntegrationTests.Infrastructure.Maps
{
	[TestFixture]
	public class AttendeeMapTester : IntegrationTestBase
	{
		[Test]
		public void Should_map_attendee_fields()
		{
			var attendee = new Attendee("Joe", "Schmoe")
			{
				Email = "foo@foo.com"
			};

			SaveEntities(attendee);

			var loaded = SessionSource.CreateSession().Load<Attendee>(attendee.Id);

			loaded.FirstName.ShouldEqual(attendee.FirstName);
			loaded.LastName.ShouldEqual(attendee.LastName);
			loaded.Email.ShouldEqual(attendee.Email);
		}
	}
}