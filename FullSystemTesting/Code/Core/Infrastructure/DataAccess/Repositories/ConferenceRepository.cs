using System;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.Core.Domain.Model;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace CodeCampServerLite.Infrastructure.DataAccess.Repositories
{
    public class ConferenceRepository : Repository<Conference>, IConferenceRepository
    {
    	public ConferenceRepository(ISession session) : base(session)
    	{
    	}

        public Conference GetByName(string eventName)
        {
            return Session.Query<Conference>().FirstOrDefault(e => e.Name == eventName);
        }
    }
}