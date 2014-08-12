using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.Core.Domain.Model;
using CodeCampServerLite.UI.Models;

namespace CodeCampServerLite.UI.Controllers
{
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using NHibernate;
    using NHibernate.Hql.Ast.ANTLR;
    using NHibernate.Linq;

    public class ConferenceIndexQuery : IRequest<ConferenceListModel[]>
    {
        public int MinSessions { get; set; }
    }

    public class ConferenceIndexQueryHandler
        : IRequestHandler<ConferenceIndexQuery, ConferenceListModel[]>
    {
        private readonly ISession _session;
        private readonly IMediator _mediator;

        public ConferenceIndexQueryHandler(ISession session, IMediator mediator)
        {
            _session = session;
            _mediator = mediator;
        }

        public ConferenceListModel[] Handle(ConferenceIndexQuery message)
        {
            var conferences = (from conf in _session.Query<Conference>()
                               where conf.SessionCount >= message.MinSessions
                               select conf).Project().To<ConferenceListModel>()
                       .ToArray();

            return conferences;
        }
    }

    public class ConferenceEditQuery : IRequest<ConferenceEditModel>
    {
        public string ConferenceName { get; set; }
    }

    public class ConferenceEditQueryHandler : IRequestHandler<ConferenceEditQuery, ConferenceEditModel>
    {
        private readonly IConferenceRepository _conferenceRepository;

        public ConferenceEditQueryHandler(IConferenceRepository conferenceRepository)
        {
            _conferenceRepository = conferenceRepository;
        }

        public ConferenceEditModel Handle(ConferenceEditQuery message)
        {
            var conf = _conferenceRepository.GetByName(message.ConferenceName);

            var model = Mapper.Map<Conference, ConferenceEditModel>(conf);

            return model;
        }
    }

    public class LoggingHandler<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;

        public LoggingHandler(IRequestHandler<TRequest, TResponse> inner)
        {
            _inner = inner;
        }

        public TResponse Handle(TRequest message)
        {
            Console.WriteLine("Before");
            var result = _inner.Handle(message);
            Console.WriteLine("After");
            return result;
        }
    }

    public class ConferenceEditHandler : RequestHandler<ConferenceEditModel>
    {
        private readonly IConferenceRepository _repository;

        public ConferenceEditHandler(IConferenceRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(ConferenceEditModel form)
        {
            var conf = _repository.GetById(form.Id);

            conf.ChangeName(form.Name);
            conf.Location = form.Location;
            conf.City = form.City;
            conf.Sponsor = form.Sponsor;
            conf.Date = DateTime.Today;
            conf.Description = form.Description;
            conf.Color = form.Color;

            foreach (var attendeeEditModel in form.Attendees)
            {
                var attendee = conf.GetAttendee(attendeeEditModel.Id);

                attendee.ChangeName(attendeeEditModel.FirstName, attendeeEditModel.LastName);
                attendee.Email = attendeeEditModel.Email;
            }

        }
    }

    public class ConferenceController : Controller
    {
        private readonly IConferenceRepository _repository;
        private readonly IMediator _mediator;

        public ConferenceController(
            IConferenceRepository repository,
            IMediator mediator
            )
        {
            _repository = repository;
            _mediator = mediator;
        }

        public ActionResult Index(int minSessions = 0)
        {
            var conferences = _mediator.Send(new ConferenceIndexQuery
            {
                MinSessions = minSessions
            });

            return View(conferences);
        }


        public ActionResult Edit(string confname)
        {
            var model = _mediator.Send(new ConferenceEditQuery
            {
                ConferenceName = confname
            });

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(ConferenceEditModel form)
        {
            _mediator.Send(form);

            return RedirectToRoute("Default", new { controller = "Conference", action = "Index" });
        }

        [HandleError(View = "NoConferenceError", ExceptionType = typeof(NullReferenceException), Order = 100)]
        public ActionResult Show(string confname)
        {
            var conf = _repository.GetByName(confname);

            var model = Mapper.Map<Conference, ConferenceShowModel>(conf);

            return View(model);
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
