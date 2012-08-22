using System;

namespace CodeCampServerLite.UI.Models
{
    public class ConferenceShowModel
    {
        public string Name { get; set; }
        public SessionModel[] Sessions { get; set; }

        public class SessionModel
        {
            public string SpeakerFirstName { get; set; }

            public string SpeakerLastName { get; set; }

            public string Title { get; set; }
        }

        public class AttendeeModel
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }
        }
    }
}
