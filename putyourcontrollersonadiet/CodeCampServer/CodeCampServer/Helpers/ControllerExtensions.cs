using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc = Microsoft.Web.Mvc.Internal;
using System.Web.Routing;
using System.Linq.Expressions;

namespace CodeCampServerLite.Helpers
{
	public static class ControllerExtensions
	{

		// Shortcut to allow users to write this.RedirectToAction(x => x.OtherMethod()) to redirect
		// to a different method on the same controller.
		public static RedirectToRouteResult RedirectToAction<TController>(this TController controller, Expression<Action<TController>> action, string routeName) where TController : Controller
		{
			return RedirectToAction<TController>((Controller)controller, action, routeName);
		}

		public static RedirectToRouteResult RedirectToActionJson<TController>(this TController controller, Expression<Action<TController>> action, string routeName) where TController : Controller
		{
			return RedirectToAction<TController>((Controller)controller, action, routeName);
		}

		public static RedirectToRouteResult RedirectToAction<TController>(this Controller controller, Expression<Action<TController>> action, string routeName) where TController : Controller
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}

			RouteValueDictionary routeValues = mvc.ExpressionHelper.GetRouteValuesFromExpression(action);
			return new RedirectToRouteResult(routeName, routeValues);
		}

	}
}