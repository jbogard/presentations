using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace CodeCampServerLite.Helpers
{
	public static class RouteExtensions
	{
		public static void ProcessArea(this RouteValueDictionary routeValues, Type targetControllerType)
		{
			var areaName = targetControllerType.GetAreaName() ?? string.Empty;

			if (routeValues.ContainsKey("area"))
				routeValues["area"] = areaName;
			else
				routeValues.Add("area", areaName);
		}
	}
}