//namespace ContosoUniversity.Services
//{
//    using Microsoft.AspNetCore.Mvc;
//    using Microsoft.AspNetCore.Mvc.Filters;
//    using Newtonsoft.Json;

//    public class ValidatorActionFilter : IActionFilter
//    {
//        public void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            if (!filterContext.ModelState.IsValid)
//            {
//                var result = new ContentResult();
//                string content = JsonConvert.SerializeObject(
//                    filterContext.ModelState, new JsonSerializerSettings
//                {
//                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//                });
//                result.Content = content;
//                result.ContentType = "application/json";

//                filterContext.HttpContext.Response.StatusCode = 400;
//                filterContext.Result = result;
//            }
//        }

//        public void OnActionExecuted(ActionExecutedContext filterContext)
//        {

//        }
//    }
//}