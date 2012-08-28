using CodeCampServerLite.Core.Domain.Model;

namespace CodeCampServerLite.Infrastructure.DataAccess.Maps
{
	public class AttendeeMap : EntityMap<Attendee>
	{
		public AttendeeMap()
		{
			Map(x => x.FirstName);
			Map(x => x.LastName);
			Map(x => x.Email);
			Map(x => x.State);

			References(x => x.Conference);
		}
	}
}