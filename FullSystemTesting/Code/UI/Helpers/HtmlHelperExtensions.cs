using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CodeCampServerLite.UI.Helpers
{
    public static class HtmlHelperExtensions
    {
		public static MvcHtmlString EmbedCurrentPage(
			this HtmlHelper helper,
			ViewContext viewContext)
		{
			var controllerName = viewContext.RouteData.Values["controller"];
			var actionName = viewContext.RouteData.Values["action"];
			var value = controllerName + "." + actionName;
			return helper.Hidden("controller-action", value);
		}
	}
}