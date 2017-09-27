using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IF.Samples.OAuth.RefreshToken.Security;
using Microsoft.Owin;

namespace IF.Samples.OAuth.RefreshToken.Controllers
{
    public class RefreshTokenController : Controller
    {
        // GET: RefreshToken
        public async Task<ActionResult> Index()
        {
            /**************************************/
            IOwinContext context = HttpContext.GetOwinContext();
            var accessCookie = HttpContext.Request.Cookies["iflo_access_token"];
            string accessToken = accessCookie == null ? null : accessCookie.Value;

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                var access = await IFOAuthRefreshTokenProvider.RefreshAccessAsync(context);
                if (access != null)
                {
                    accessToken = access.AccessToken;

                    ViewBag.AccessToken = access.AccessToken;
                    ViewBag.RefreshToken = access.RefreshToken;
                    ViewBag.ExpiresIn = access.ExpiresIn;
                }
            }
            else
            {
                ViewBag.AccessToken = accessToken;
                ViewBag.RefreshToken = HttpContext.Request.Cookies["iflo_refresh_token"].Value;
            }
            // can now use access token for calls to the API
            /**************************************/
            
            ViewBag.Message = string.IsNullOrEmpty(accessToken) ? "You need to be logged in and have a refresh token to obtain an access token." : "Access token and refresh token information.";

            return View();
        }
    }
}