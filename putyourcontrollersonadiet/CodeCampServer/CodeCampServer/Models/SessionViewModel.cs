using System;

namespace CodeCampServerLite.Models
{
    public class SessionViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Speaker { get; set; }
    }
}