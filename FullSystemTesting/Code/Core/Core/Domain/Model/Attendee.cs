namespace CodeCampServerLite.Core.Domain.Model
{
    public class Attendee : Entity
    {
    	public Attendee(string firstName, string lastName)
    	{
    		FirstName = firstName;
    		LastName = lastName;
    	}

		protected Attendee() { }

    	public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
    	public virtual string Email { get; set; }
        public virtual string State { get; set; }
        public virtual Conference Conference { get; protected set; }

        public virtual void ChangeName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

    	public virtual void RegisterFor(Conference conference)
    	{
    		Conference = conference;
    		conference.AddAttendee(this);
    	}
    }
}