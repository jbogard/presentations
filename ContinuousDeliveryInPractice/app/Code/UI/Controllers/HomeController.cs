using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeCampServerLite.Core.Domain.Model;

namespace CodeCampServerLite.UI.Controllers
{
    using Helpers;
    using StructureMap;

    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ViewResult BlowUp()
        {
            throw new Exception("Custom message");
        }


        public void Reset()
        {
            var loader = ObjectFactory.GetInstance<IDummyDataLoader>();

            loader.Load();
        }


        [ChildActionOnly]
        public PartialViewResult Version()
        {
            var version = typeof(Conference).Assembly.GetName().Version;

            return PartialView(version);
        }
    }
}
