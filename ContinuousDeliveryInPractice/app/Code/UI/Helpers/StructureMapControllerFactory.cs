using System.Web.Mvc;
using StructureMap;

namespace CodeCampServerLite.UI.Helpers
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer _container;

        public StructureMapControllerFactory(IContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, System.Type controllerType)
        {
            if (controllerType == null)
                return null;

            object controller = _container.GetInstance(controllerType);

            return (IController)controller;
        }
    }
}