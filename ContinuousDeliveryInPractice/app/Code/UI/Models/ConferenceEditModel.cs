using System;

namespace CodeCampServerLite.UI.Models
{
    public class ConferenceEditModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }

        public AttendeeEditModel[] Attendees { get; set; }

        public class AttendeeEditModel
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }
    }
}