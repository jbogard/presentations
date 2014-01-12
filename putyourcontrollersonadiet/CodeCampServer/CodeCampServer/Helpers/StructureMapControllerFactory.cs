using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace CodeCampServerLite.Helpers
{
	public class StructureMapControllerFactory : DefaultControllerFactory
	{
		protected override IController GetControllerInstance(
			RequestContext requestContext, 
			Type controllerType)
		{
			if (controllerType == null)
				return null;

			object controller = ObjectFactory.GetInstance(controllerType);

			return (IController)controller;
		}
	}
}