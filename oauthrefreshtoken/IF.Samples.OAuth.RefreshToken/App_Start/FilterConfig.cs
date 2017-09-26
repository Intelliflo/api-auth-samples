using System.Web;
using System.Web.Mvc;

namespace IF.Samples.OAuth.RefreshToken
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
