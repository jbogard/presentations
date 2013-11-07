using System;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.Core.Domain.Model;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace CodeCampServerLite.Infrastructure.DataAccess.Repositories
{
    using Core.Domain;
    using Core.Domain.Model;

    public class ConferenceRepository : Repository<Conference>, IConferenceRepository
    {
    	public ConferenceRepository(ISession session) : base(session)
    	{
    	}

        public Conference GetByName(string eventName)
        {
            return Session.Linq<Conference>().FirstOrDefault(e => e.Name == eventName);
        }
    }
}