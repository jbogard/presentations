using System.Web.Mvc;
using System.Web.Routing;
using CodeCampServerLite;
using CodeCampServerLite.UI.Helpers;
using StructureMap;

namespace UI
{
    using CodeCampServerLite.UI;
    using CodeCampServerLite.UI.App_Start;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "ViewConference",
               "Conference/{confname}/{action}",
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
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

            AutoMapperBootstrapper.Initialize();

            StructuremapMvc.Start();
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory(IoC.Container));
        }
    }
}