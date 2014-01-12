using System;
using System.Web.Mvc;
using CodeCampServerLite.Helpers.XmlSerialization;

namespace CodeCampServerLite.Helpers
{
    public class XmlResult<T> : ActionResult
    {
        public T ModelToSerialize { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var contentResult = new ContentResult();
            contentResult.Content = ModelToSerialize.Serialize();
            contentResult.ContentType = "text/xml";
            contentResult.ExecuteResult(context);
        }
    }
}
