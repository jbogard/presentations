using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace CodeCampServerLite.Core.Domain.Model
{
    public class Speaker : Entity
    {
    	private readonly Iesi.Collections.Generic.ISet<Session> _sessions = new HashedSet<Session>();

    	public Speaker(string firstName, string lastName)
    	{
    		FirstName = firstName;
    		LastName = lastName;
    	}

		protected Speaker() {}

        public virtual string FirstName { get; protected set; }

        public virtual string LastName { get; protected set; }

    	public virtual string Bio { get; set; }

    	public virtual IEnumerable<Session> GetSessions()
    	{
    		return _sessions;
    	}

    	public virtual void Register(Session session)
    	{
    		_sessions.Add(session);
    	}
    }
}