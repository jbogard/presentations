using System.Linq;
using CodeCampServerLite.Core.Domain.Model;
using Should;
using NUnit.Framework;

namespace CodeCampServerLite.IntegrationTests.Infrastructure.Maps
{
    [TestFixture]
    public class ConferenceMapTester : IntegrationTestBase
    {
        [Test]
        public void Should_map_all_event_fields_correctly()
        {
            var newEvent = new Conference("Some event");

        	SaveEntities(newEvent);

            var newSession = SessionSource.CreateSession();

            var savedEvent = newSession.Load<Conference>(newEvent.Id);

            savedEvent.Name.ShouldEqual(newEvent.Name);
        }

    	[Test]
    	public void Should_cascade_session()
    	{
			var newEvent = new Conference("Some event");
    		var session = new Session("Foo", "Bar", new Speaker("Joe", "Schmoe"));

			newEvent.AddSession(session);

    		SaveEntities(newEvent);

    		var loaded = SessionSource.CreateSession().Load<Conference>(newEvent.Id);

    	    loaded.SessionCount.ShouldEqual(1);
    		loaded.GetSessions().Count().ShouldEqual(1);
    		loaded.GetSessions().ElementAt(0).Conference.ShouldEqual(loaded);
    	}

    	[Test]
    	public void Should_cascade_attendee()
    	{
    		var attendee = new Attendee("Joe", "Schmoe");
			var newEvent = new Conference("Some event");
			
			attendee.RegisterFor(newEvent);

    		SaveEntities(newEvent);

			var loaded = SessionSource.CreateSession().Load<Conference>(newEvent.Id);

    		loaded.GetAttendees().Count().ShouldEqual(1);
    		loaded.GetAttendees().ElementAt(0).Conference.ShouldEqual(loaded);
		}
    }
}