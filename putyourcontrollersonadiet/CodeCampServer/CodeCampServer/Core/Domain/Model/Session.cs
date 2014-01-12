using System;

namespace CodeCampServerLite.Core.Domain.Model
{
    public class Session : Entity
    {
    	public Session(string title, string @abstract, Speaker speaker)
    	{
    		Title = title;
    		Abstract = @abstract;
    		Speaker = speaker;

    		speaker.Register(this);
    	}

		protected Session() {}

        public virtual string Title { get; protected set; }
        public virtual string Abstract { get; protected set; }
        public virtual Speaker Speaker { get; protected set; }
    	public virtual Conference Conference { get; protected internal set; }
    }
}