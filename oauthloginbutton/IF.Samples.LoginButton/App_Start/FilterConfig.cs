using System.Web;
using System.Web.Mvc;

namespace IF.Samples.LoginButton
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
