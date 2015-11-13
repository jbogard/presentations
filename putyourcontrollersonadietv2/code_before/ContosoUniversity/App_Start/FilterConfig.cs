using System.Web;
using System.Web.Mvc;

namespace ContosoUniversity
{
    using Helpers;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MvcTransactionFilter());
            filters.Add(new ValidatorActionFilter());
        }
    }
}
