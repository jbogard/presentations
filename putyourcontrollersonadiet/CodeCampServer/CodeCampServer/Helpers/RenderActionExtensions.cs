using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Web.Routing;

namespace CodeCampServerLite.Helpers
{
	public static class RenderActionExtensions
	{
		public static void RenderAction<TController>(this HtmlHelper helper, Expression<Action<TController>> action)
			where TController : Controller
		{
			helper.RenderAction<TController>(action, null);
		}

		public static void RenderAction<TController>(this HtmlHelper helper, Expression<Action<TController>> action,
													 RouteValueDictionary values)
			where TController : Controller
		{
			values = values ?? new RouteValueDictionary();

			Type type = typeof(TController);
			string controllerName = type.GetControllerName();
			string actionName = action.GetActionName();

			values.ProcessArea(type);

			helper.RenderAction(actionName, controllerName, values);
		}
	}
}