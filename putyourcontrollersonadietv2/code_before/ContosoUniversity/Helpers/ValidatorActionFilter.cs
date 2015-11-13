namespace ContosoUniversity.Helpers
{
    using System.Web.Mvc;
    using Newtonsoft.Json;

    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                var result = new ContentResult();
                string content = JsonConvert.SerializeObject(filterContext.Controller.ViewData.ModelState, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                result.Content = content;
                result.ContentType = "application/json";

                filterContext.HttpContext.Response.StatusCode = 400;
                filterContext.Result = result;
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}