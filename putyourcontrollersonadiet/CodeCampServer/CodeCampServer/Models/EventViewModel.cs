using System;
using System.Collections.Generic;

namespace CodeCampServerLite.Models
{
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public string EventName { get; set; }

        public List<AttendeeViewModel> Attendees { get; set; }
        public List<SessionViewModel> Sessions { get; set; }
    }
}