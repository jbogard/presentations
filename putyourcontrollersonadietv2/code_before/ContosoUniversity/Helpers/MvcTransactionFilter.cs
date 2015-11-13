namespace ContosoUniversity.Helpers
{
    using System.Web.Mvc;
    using App_Start;
    using DAL;
    using EntityFramework.Extensions;

    public class MvcTransactionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer.GetInstance<SchoolContext>();
            context.BeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var instance = StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer.GetInstance<SchoolContext>();
            instance.CloseTransaction(filterContext.Exception);
        }
    }
}