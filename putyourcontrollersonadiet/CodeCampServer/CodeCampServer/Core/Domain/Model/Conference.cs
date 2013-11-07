using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Iesi.Collections.Generic;
using System.Linq;

namespace CodeCampServerLite.Core.Domain.Model
{
    public class Conference : Entity
    {
    	private readonly Iesi.Collections.Generic.ISet<Attendee> _attendees = new HashedSet<Attendee>();
    	private readonly Iesi.Collections.Generic.ISet<Session> _sessions = new HashedSet<Session>();

    	public Conference(string name)
    	{
    		Name = name;
    	    AttendeeCount = 0;
            SessionCount = 0;
    	}

    	protected Conference() { }

        public virtual string Name { get; protected set; }
        public virtual int AttendeeCount { get; protected set; }
        public virtual int SessionCount { get; protected set; }

        public virtual void ChangeName(string name)
        {
            Name = name;
        }

    	public virtual IEnumerable<Attendee> GetAttendees()
    	{
            return _attendees;
    	}

        public virtual Attendee GetAttendee(Guid attendeeId)
        {
            return _attendees.First(a => a.Id == attendeeId);
        }

    	public virtual void AddSession(Session session)
    	{
    		_sessions.Add(session);
    		session.Conference = this;
    	    SessionCount++;
    	}

        public virtual IEnumerable<Session> GetSessions()
    	{
            return _sessions;
    	}

    	protected internal virtual void AddAttendee(Attendee attendee)
    	{
    		_attendees.Add(attendee);
    	    AttendeeCount++;
    	}
    }
}