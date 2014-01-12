using FluentNHibernate.Mapping;
using CodeCampServerLite.Core.Domain.Model;

namespace CodeCampServerLite.Infrastructure.DataAccess.Maps
{
    using Core.Domain.Model;

    public class SpeakerMap : EntityMap<Speaker>
	{
		public SpeakerMap()
		{
			Map(x => x.FirstName);
			Map(x => x.LastName);
			Map(x => x.Bio);

			HasMany(x => x.GetSessions())
				.AsSet()
				//.Access.CamelCaseField(Prefix.Underscore)
                ;
		}
	}
}