using System;

namespace CodeCampServerLite.Models
{
    public class ConferenceListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int SessionCount { get; set; }
		public int AttendeeCount { get; set; }
    }
}