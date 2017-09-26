using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.Owin;

namespace IF.Samples.OAuth.RefreshToken.Security
{
    public static class IOwinContextExtensions
    {
        public static void PersistAccessTokens(this IOwinContext context, string accessToken, string refreshToken, string expiresIn)
        {
            TimeSpan lifespan = TimeSpan.FromSeconds(Int32.Parse(expiresIn, CultureInfo.InvariantCulture));

            CookieOptions accessCookieOptions = new CookieOptions();
            //accessCookieOptions.Expires = DateTime.UtcNow.Add(lifespan); // TODO restore
            accessCookieOptions.Expires = DateTime.UtcNow.AddSeconds(30); // TODO remove
            accessCookieOptions.HttpOnly = true;
            accessCookieOptions.Secure = true;

            CookieOptions refreshCookieOptions = new CookieOptions();
            refreshCookieOptions.HttpOnly = true;
            refreshCookieOptions.Secure = true;

            context.Response.OnSendingHeaders(persistState =>
            {
                // store these values in cookies for use later
                context.Response.Cookies.Append("iflo_access_token", accessToken, accessCookieOptions);
                // this cookie "should" expire when access expires
                context.Response.Cookies.Append("iflo_refresh_token", refreshToken, refreshCookieOptions);

                // could use session, database, claims ....
            }, null);
        }
    }
}