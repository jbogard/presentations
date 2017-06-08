using System.Web;
using System.Web.Mvc;

namespace Polyglot.UI.Orders
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}