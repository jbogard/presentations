using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CodeCampServerLite.Models
{
    public class AttendeeEditModel
    {
        [HiddenInput(DisplayValue = false)]
        public Guid EventId { get; set; }

        [HiddenInput]
        public string EventName { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        [RegularExpression(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$", ErrorMessage = "Not a valid Email")]
        public string Email { get; set; }
    }
}
