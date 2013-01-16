using System;
using System.Web.Mvc;

namespace CodeCampServerLite.UI.Models
{
    public class ConferenceListModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }
        public int SessionCount { get; set; }
        public int AttendeeCount { get; set; }
        public Guid Id { get; set; }
    }
}