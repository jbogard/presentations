using System.Linq;
using System.Web.Mvc;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.Helpers;
using CodeCampServerLite.Models;
using CodeCampServerLite.Helpers.XmlSerialization;

namespace CodeCampServerLite.Controllers
{
    using Helpers;
    using Models;

    public class ConferenceXmlController : DefaultController
    {
        private readonly IConferenceRepository _repository;

        public ConferenceXmlController(IConferenceRepository repository)
        {
            _repository = repository;
        }

		// Before custom action result

		//public ContentResult Index()
		//{
		//    var list = BuildModel();

		//    var contentResult = new ContentResult
		//    {
		//        Content = list.Serialize(),
		//        ContentType = "text/xml"
		//    };

		//    return contentResult;
		//}

		public XmlResult<ConferenceXmlModel[]> Index()
		{
			var list = BuildModel();

			return Xml(list);
		}

    	private ConferenceXmlModel[] BuildModel()
    	{
    		return _repository.GetAll()
    			.Select(e => new ConferenceXmlModel
    			             	{
    			             		EventName = e.Name,
    			             		AttendeeCount = e.GetAttendees().Count().ToString(),
    			             		SessionCount = e.GetSessions().Count().ToString()
    			             	})
    			.ToArray();
    	}
    }
}