namespace CodeCampServerLite.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Core.Domain.Model;
    using Helpers;
    using Models;
    using NHibernate;
    using NHibernate.Linq;
    using NHibernate.Mapping;
    using StructureMap;

    public interface IMediator
    {
        TResponse Request<TResponse>(IQuery<TResponse> query);
    }

    public class Mediator : IMediator
    {
        readonly IContainer _container;

        public Mediator(IContainer container)
        {
            _container = container;
        }


        public TResponse Request<TResponse>(IQuery<TResponse> query)
        {
            var handler = GetHandler(query);

            TResponse result = ProcessQueryWithHandler(query, handler);

            return result;
        }

        object GetHandler<TResponse>(IQuery<TResponse> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
            var handler = _container.GetInstance(handlerType);
            return handler;
        }

        TResponse ProcessQueryWithHandler<TResponse>(IQuery<TResponse> query, object handler)
        {
            return (TResponse)handler.GetType().GetMethod("Handle").Invoke(handler, new object[] { query });
        }
    }

    public interface IQuery<out TResponse> { }

    public interface IQueryHandler<in TQuery, out TResponse>
        where TQuery : IQuery<TResponse>
    {
        TResponse Handle(TQuery query);
    }

    public class ShowQuery
        : IQuery<ConferenceShowModel>
    {
        public string EventName { get; set; }
    }

public class ShowQueryHandler 
    : IQueryHandler<ShowQuery, ConferenceShowModel>
    {
        private readonly ISession _session;

        public ShowQueryHandler(ISession session)
        {
            _session = session;
        }

        public ConferenceShowModel Handle(ShowQuery query)
        {
            var model = _session
                .Query<Conference>()
                .Where(c => c.Name == query.EventName)
                .Project().To<ConferenceShowModel>()
                .SingleOrDefault();

            return model;
        }
    }

    public class IndexQuery : IQuery<ConferenceListModel[]>
    {
        public int? MinSessions { get; set; }
    }

    public class IndexQueryHandler : IQueryHandler<IndexQuery, ConferenceListModel[]>
    {
        private readonly ISession _session;

        public IndexQueryHandler(ISession session)
        {
            _session = session;
        }

        public ConferenceListModel[] Handle(IndexQuery query)
        {
            var minSessions = query.MinSessions ?? 0;

            var list = _session
                .Query<Conference>()
                .Where(c => c.SessionCount >= minSessions)
                .Project().To<ConferenceListModel>()
                .ToArray();

            return list;
        }
    }

    public class EditQuery : IQuery<ConferenceEditModel>
    {
        public string EventName { get; set; }
    }

    public class EditQueryHandler : IQueryHandler<EditQuery, ConferenceEditModel>
    {
        private readonly ISession _session;

        public EditQueryHandler(ISession session)
        {
            _session = session;
        }

        public ConferenceEditModel Handle(EditQuery query)
        {
            var model = _session
                .Query<Conference>()
                .Where(c => c.Name == query.EventName)
                .Project().To<ConferenceEditModel>()
                .SingleOrDefault();

            return model;
        }
    }

public class ConferenceController : Controller
{
    private readonly ISession _session;
    private readonly IMediator _mediator;

    public ConferenceController(ISession session, IMediator mediator)
    {
        _session = session;
        _mediator = mediator;
    }

    public ActionResult Index(IndexQuery query)
    {
        var model = _mediator.Request(query);

        return View(model);
    }

    public ViewResult Show(ShowQuery query)
    {
        var model = _mediator.Request(query);

        return View(model);
    }

    public ActionResult Edit(EditQuery query)
    {
        var model = _mediator.Request(query);

        return View(model);
    }


        [HttpPost]
        public ActionResult Edit(ConferenceEditModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var conf = _session.Get<Conference>(form.Id);

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