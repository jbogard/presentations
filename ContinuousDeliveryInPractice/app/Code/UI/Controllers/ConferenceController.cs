using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.Core.Domain.Model;
using CodeCampServerLite.UI.Models;

namespace CodeCampServerLite.UI.Controllers
{
    using Helpers;
    using StructureMap;

    public class ConferenceController : DefaultController
    {
        private readonly IConferenceRepository _repository;

        public ConferenceController(IConferenceRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index(int minSessions = 0)
        {
            var conferences = (from conf in _repository.Query()
                       where conf.SessionCount >= minSessions
                       select conf).ToArray();

            return AutoMapView<ConferenceListModel[]>(conferences, View());
        }


        public ActionResult Edit(string confname)
        {
            var conf = _repository.GetByName(confname);

            return AutoMapView<ConferenceEditModel>(conf, View());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(ConferenceEditModel form)
        {
            var conf = _repository.GetById(form.Id);

            conf.ChangeName(form.Name);
            conf.Location = form.Location;
            conf.City = form.City;
            conf.Date = DateTime.Today;

            foreach (var attendeeEditModel in form.Attendees)
            {
                var attendee = conf.GetAttendee(attendeeEditModel.Id);

                attendee.ChangeName(attendeeEditModel.FirstName, attendeeEditModel.LastName);
                attendee.Email = attendeeEditModel.Email;
            }

            return RedirectToRoute("Default", new { controller = "Conference", action = "Index" });
        }

        [HandleError(View = "NoConferenceError", ExceptionType = typeof(NullReferenceException), Order = 100)]
        public ActionResult Show(string confname)
        {
            var conf = _repository.GetByName(confname);

            return AutoMapView<ConferenceShowModel>(conf, View());
        }

        public ActionResult Register(string confname)
        {
            var conf = _repository.GetByName(confname);

            return View(new AttendeeEditModel
            {
                ConferenceId = conf.Id,
                ConferenceName = conf.Name
            });
        }

        [HttpPost]
        public ActionResult Register(AttendeeEditModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _repository.GetById(model.ConferenceId);
                var attendee = new Attendee(model.FirstName, model.LastName)
                {
                    Email = model.Email
                };
                attendee.RegisterFor(entity);
                return RedirectToAction("AttendeeConfirmation", model);
            }

            return View(model);
        }

        public ActionResult AttendeeConfirmation(AttendeeEditModel model)
        {
            return View(model);
        }
    }
}
