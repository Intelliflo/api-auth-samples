using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace IF.Samples.OAuth.RefreshToken.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.SessionId = Session.SessionID;
            ViewBag.User = User.Identity;
            
            /******************/
            ViewBag.Code = Request.QueryString["code"] ?? "";
            ViewBag.Error = Request.QueryString["error"] ?? "";
            if (!string.IsNullOrEmpty(ViewBag.Code))
            {
                // TODO: hack until I can update developer portal redirect URL
                return RedirectToAction("ExternalLoginCallback", "Account");
            }
            /******************/

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}