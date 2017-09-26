using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IF.Samples.OAuth.LoginButton.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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