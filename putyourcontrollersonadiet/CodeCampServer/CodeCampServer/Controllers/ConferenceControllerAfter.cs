using System.Web.Mvc;
using CodeCampServerLite.Core.Domain.Model;
using CodeCampServerLite.Core.Domain.Query;
using CodeCampServerLite.Helpers;
using CodeCampServerLite.Models;

namespace CodeCampServerLite.Controllers
{
    using Models;

    public class ConferenceAfterController : DefaultController
    {
		public ActionResult Index(int? minSessions)
		{
			return Query<ConferenceListModel[]>(View(new ListConferences(minSessions)));
		}

		public ActionResult Show(Conference eventName)
		{
			return AutoMapView<ConferenceShowModel>(View(eventName));
		}

		public ActionResult Edit(Conference eventname)
        {
			return AutoMapView<ConferenceEditModel>(View(eventname));
        }

        [HttpPost]
        public ActionResult Edit(ConferenceEditModel form)
        {
			var success = this.RedirectToAction(c => c.Index(null), "Default");
			
			return Form(form, success);
        }
    }
}
