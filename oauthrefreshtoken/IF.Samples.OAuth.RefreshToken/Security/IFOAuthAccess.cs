using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace IF.Samples.OAuth.RefreshToken.Security
{
    /// <summary>
    /// Encapsulates an OAuth token
    /// </summary>
    public class IFOAuthAccess
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiresIn { get; set; }

        public IFOAuthAccess(JObject oauthToken)
        {
            AccessToken = oauthToken["access_token"].Value<string>();

            // Refresh token is only available when offline access (offline_access) is requested.
            // Otherwise, it is null.
            RefreshToken = oauthToken.Value<string>("refresh_token");
            ExpiresIn = oauthToken.Value<string>("expires_in");
        }

        /// <summary>
        /// Currently persists to cookies
        /// </summary>
        /// <param name="context">Used to gain access to the cookie collection</param>
        /// <remarks>Could replace this method with one that writes to session, database, claims .... etc.</remarks>
        public void Persist(IOwinContext context)
        {
            TimeSpan lifespan = TimeSpan.FromSeconds(Int32.Parse(this.ExpiresIn, CultureInfo.InvariantCulture));

            CookieOptions accessCookieOptions = new CookieOptions();
            accessCookieOptions.Expires = DateTime.UtcNow.Add(lifespan); 
            accessCookieOptions.HttpOnly = true;
            accessCookieOptions.Secure = true;

            CookieOptions refreshCookieOptions = new CookieOptions();
            refreshCookieOptions.HttpOnly = true;
            refreshCookieOptions.Secure = true;

            context.Response.OnSendingHeaders(persistState =>
            {
                // store these values in cookies for use later
                context.Response.Cookies.Append("iflo_access_token", this.AccessToken, accessCookieOptions);
                // this cookie "should" expire when access expires
                context.Response.Cookies.Append("iflo_refresh_token", this.RefreshToken, refreshCookieOptions);
            }, null);
        }
    }
}