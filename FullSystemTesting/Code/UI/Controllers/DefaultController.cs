using System.Web.Mvc;
using CodeCampServerLite.UI.Helpers;

namespace CodeCampServerLite.UI.Controllers
{
    public abstract class DefaultController : Controller
    {
        protected XmlResult<T> Xml<T>(T toSerialize)
        {
            return new XmlResult<T>
            {
                ModelToSerialize = toSerialize
            };
        }

		protected AutoMapViewResult AutoMapView<TDestination>(object domainModel, ViewResultBase viewResult)
        {
            return new AutoMapViewResult(domainModel.GetType(), typeof(TDestination), domainModel, viewResult);
        }
    }
}
