using System.Web.Mvc;

namespace CodeCampServerLite.Controllers
{
    public class DynamicController : Controller
    {
        public ActionResult Index(string text)
        {
            ViewData["text"] = text;
            return View();
        }

        public RedirectResult Goto(string gotoUrl)
        {
            return Redirect(gotoUrl);
        }

    }
}
