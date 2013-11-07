using FluentNHibernate.Mapping;
using CodeCampServerLite.Core.Domain.Model;

namespace CodeCampServerLite.Infrastructure.DataAccess.Maps
{
    using Core.Domain.Model;

    public class ConferenceMap : EntityMap<Conference>
    {
        public ConferenceMap()
        {
            Map(x => x.Name);
            Map(x => x.SessionCount);
            Map(x => x.AttendeeCount);


        	HasMany(x => x.GetSessions())
        		.AsSet()
        		//.Access.CamelCaseField(Prefix.Underscore)
        		.Cascade.AllDeleteOrphan();

        	HasMany(x => x.GetAttendees())
        		.AsSet()
        		//.Access.CamelCaseField(Prefix.Underscore)
        		.Cascade.AllDeleteOrphan();
        }
    }
}