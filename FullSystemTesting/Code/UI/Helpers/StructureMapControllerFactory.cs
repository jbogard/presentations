using System.Web.Mvc;
using StructureMap;

namespace CodeCampServerLite.UI.Helpers
{
	public class StructureMapControllerFactory : DefaultControllerFactory
	{
		protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, System.Type controllerType)
		{
			if (controllerType == null)
				return null;

			object controller = ObjectFactory.GetInstance(controllerType);

			return (IController)controller;
		}
	}
}