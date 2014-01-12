using CodeCampServerLite.Core.Domain.Model;

namespace CodeCampServerLite.Infrastructure.DataAccess.Maps
{
    using Core.Domain.Model;

    public class AttendeeMap : EntityMap<Attendee>
	{
		public AttendeeMap()
		{
			Map(x => x.FirstName);
			Map(x => x.LastName);
			Map(x => x.Email);

			References(x => x.Conference);
		}
	}
}