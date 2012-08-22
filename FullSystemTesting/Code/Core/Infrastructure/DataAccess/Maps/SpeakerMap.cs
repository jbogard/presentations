using CodeCampServerLite.Core.Domain.Model;
using FluentNHibernate.Mapping;

namespace CodeCampServerLite.Infrastructure.DataAccess.Maps
{
	public class SpeakerMap : EntityMap<Speaker>
	{
		public SpeakerMap()
		{
			Map(x => x.FirstName);
			Map(x => x.LastName);
			Map(x => x.Bio);

			HasMany(x => x.GetSessions())
				.AsSet();
		}
	}
}