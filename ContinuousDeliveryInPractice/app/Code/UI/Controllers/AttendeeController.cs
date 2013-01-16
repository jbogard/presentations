using System.Linq;
using System.Web.Mvc;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.UI.Models;

namespace CodeCampServerLite.UI.Controllers
{
    public class AttendeeController : DefaultController
    {
        private readonly IConferenceRepository _repository;

        public AttendeeController(IConferenceRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Show(string confname)
        {
            var conference = _repository.GetByName(confname);
			var attendees = conference.GetAttendees().OrderBy(x => x.FirstName).ThenBy(x => x.LastName);

            return AutoMapView<ConferenceShowModel.AttendeeModel[]>(attendees, PartialView("_Show"));
		}
    }
}