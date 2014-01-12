using System;
using System.Linq;
using CodeCampServerLite.Core.Domain.Model;
using NHibernate.Linq;

namespace CodeCampServerLite.Core.Domain.Query
{
    using Model;

    public class ListConferences : Query<Conference, Conference[]>
	{
		private readonly int _minSessions;

		public ListConferences(int? minSessions)
		{
			_minSessions = minSessions ?? 0;
		}

		public override Conference[] Execute(INHibernateQueryable<Conference> queryProvider)
		{
			return (from conf in queryProvider
				   where conf.SessionCount >= _minSessions
			       select conf).ToArray();
		}
	}
}