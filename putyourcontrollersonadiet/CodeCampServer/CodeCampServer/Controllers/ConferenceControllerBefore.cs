using System.Linq;
using System.Web.Mvc;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.Infrastructure.DataAccess;
using CodeCampServerLite.Infrastructure.DataAccess.Repositories;
using CodeCampServerLite.Models;
using CodeCampServerLite.Helpers;

namespace CodeCampServerLite.Controllers
{
    using Models;

    public class ConferenceControllerBefore : DefaultController
    {
		private readonly IConferenceRepository _repository;

		public ConferenceControllerBefore()
		{
			_repository = new ConferenceRepository(Sessions.Current);
		}

		public ActionResult Index(int? minSessions)
		{
			minSessions = minSessions ?? 0;

			var list = (from conf in _repository.Query()
						where conf.SessionCount >= minSessions
						select new ConferenceListModel
						{
							Id = conf.Id,
							Name = conf.Name,
							SessionCount = conf.SessionCount,
							AttendeeCount = conf.AttendeeCount
						}).ToArray();

			return View(list);
		}

		public ViewResult Show(string eventName)
		{
			var conf = _repository.GetByName(eventName);

			var model = new ConferenceShowModel
			{
				Name = conf.Name,
				Sessions = conf.GetSessions()
					.Select(s => new ConferenceShowModel.SessionModel
					{
						Title = s.Title,
						SpeakerFirstName = s.Speaker.FirstName,
						SpeakerLastName = s.Speaker.LastName,
					}).ToArray()
			};

			return View(model);
		}

		public ActionResult Edit(string eventName)
		{
			var conf = _repository.GetByName(eventName);

			var model = new ConferenceEditModel
			{
				Id = conf.Id,
				Name = conf.Name,
				Attendees = conf.GetAttendees()
					.Select(a => new ConferenceEditModel.AttendeeEditModel
					{
						Id = a.Id,
						FirstName = a.FirstName,
						LastName = a.LastName,
						Email = a.Email,
					}).ToArray()
			};

			return View(model);
		}


		[HttpPost]
		public ActionResult Edit(ConferenceEditModel form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}

			var conf = _repository.GetById(form.Id);

			conf.ChangeName(form.Name);

			foreach (var attendeeEditModel in form.Attendees)
			{
				var attendee = conf.GetAttendee(attendeeEditModel.Id);

				attendee.ChangeName(attendeeEditModel.FirstName, attendeeEditModel.LastName);
				attendee.Email = attendeeEditModel.Email;
			}

			return this.RedirectToAction(c => c.Index(null), "Default");
		}
    }


}
