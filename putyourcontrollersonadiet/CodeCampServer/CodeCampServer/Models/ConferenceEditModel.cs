using System;

namespace CodeCampServerLite.Models
{
    using Controllers;
    using Core.Domain.Model;

    public class ConferenceEditModel : ICommand<Conference>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

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