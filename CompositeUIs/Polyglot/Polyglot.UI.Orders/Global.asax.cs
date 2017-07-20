using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NServiceBus;
using Raven.Client;
using Raven.Client.Document;

namespace Polyglot.UI.Orders
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static DocumentStore DocumentStore;

        protected void Application_Start()
        {
            DocumentStore = new DocumentStore
            {
                DefaultDatabase = "Polyglot.UI.Orders",
                Url = "http://localhost:8080"
            };
            DocumentStore.Initialize();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Bus = Configure
                .With()
                .DefaultBuilder()
                .XmlSerializer()
                .MsmqTransport()
                .UnicastBus()
                .SendOnly();
        }

        public static IBus Bus { get; set; }
    }
}