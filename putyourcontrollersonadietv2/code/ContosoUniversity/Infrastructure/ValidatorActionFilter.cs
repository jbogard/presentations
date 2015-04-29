namespace ContosoUniversity.Infrastructure
{
    using System.Net;
    using System.Web.Mvc;
    using Newtonsoft.Json;

    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                if (filterContext.HttpContext.Request.HttpMethod == "GET")
                {
                    var result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    filterContext.Result = result;
                }
                else
                {
                    var result = new ContentResult();
                    string content = JsonConvert.SerializeObject(filterContext.Controller.ViewData.ModelState,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    result.Content = content;
                    result.ContentType = "application/json";

                    filterContext.HttpContext.Response.StatusCode = 400;
                    filterContext.Result = result;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}