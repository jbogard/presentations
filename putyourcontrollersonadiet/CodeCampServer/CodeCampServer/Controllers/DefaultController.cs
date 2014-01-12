using System;
using System.Web.Mvc;
using CodeCampServerLite.Core.Domain.Model;
using CodeCampServerLite.Helpers;

namespace CodeCampServerLite.Controllers
{
    using Helpers;

    public class DefaultController : Controller
    {
    	protected FormActionResult<TForm> Form<TForm>(
    		TForm form, 
    		ActionResult success)
    	{
    		var failure = View(form);

    		return new FormActionResult<TForm>(form, success, failure);
    	}

    	protected AutoMapViewResult AutoMapView<TDestination>(ViewResult viewResult)
        {
            return new AutoMapViewResult(
				viewResult.ViewData.Model.GetType(), 
				typeof(TDestination), 
				viewResult);
        }

    	#region Other helper methods
		protected XmlResult<T> Xml<T>(T toSerialize)
		{
			return new XmlResult<T>
			{
				ModelToSerialize = toSerialize
			};
		}

		protected ActionResult Query<TDestination>(ViewResult viewResult)
		{
			return viewResult;
		}
		#endregion
    }
}
