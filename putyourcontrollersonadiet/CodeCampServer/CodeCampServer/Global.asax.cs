using System.Web.Mvc;
using System.Web.Routing;
using CodeCampServerLite.Helpers;
using StructureMap;
using CodeCampServerLite.Core.Domain.Model;

namespace CodeCampServerLite
{
    using CodeCampServerLite;
    using CodeCampServerLite.Core.Domain.Model;
    using CodeCampServerLite.Helpers;
    using Helpers;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "ViewEvent",
                "Conference/{eventname}/{action}",
                new { controller = "Conference", action = "Show" });

			routes.MapRoute(
				"Default",                                              // Route name
				"{controller}/{action}/{id}",                           // URL with parameters
				new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
			);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

			ModelMetadataProviders.Current = new ConventionProvider();

			StructureMapBootStrapper.Bootstrap();

            ControllerBuilder.Current
				.SetControllerFactory(new StructureMapControllerFactory());

			AutoMapperBootstrapper.Initialize();

			ModelBinders.Binders
				.Add(typeof(Conference), new ConferenceModelBinder());

			var dataLoader = ObjectFactory.GetInstance<IDummyDataLoader>();

			dataLoader.Load();
		}
	}

	public interface IFilteredModelBinder
		: IModelBinder
	{
		bool IsMatch(ModelBindingContext bindingContext);
	}
}