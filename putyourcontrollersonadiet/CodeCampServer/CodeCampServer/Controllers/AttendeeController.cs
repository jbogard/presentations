using System;
using System.Web.Mvc;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.Models;

namespace CodeCampServerLite.Controllers
{
    public class AttendeeController : DefaultController
    {
        private readonly IConferenceRepository _repository;

        public AttendeeController(IConferenceRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Show(string eventName)
        {
            var e = _repository.GetByName(eventName);

            return null;
        }
    }
}