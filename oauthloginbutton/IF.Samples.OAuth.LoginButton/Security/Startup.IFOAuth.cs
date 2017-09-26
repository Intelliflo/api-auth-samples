using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;

namespace IF.Samples.OAuth.LoginButton.Security
{
    public static class Startup
    {
        /// <summary>
        /// Configure the application to authenticate using IO
        /// </summary>
        public static void ConfigureAuthWithIntelliflo(IAppBuilder app)
        {
            // Client Id from the Intelliflo developer portal
            string clientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"];

            // Client secret from Intelliflo developer portal
            string clientSecret = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];

            // RedirectUri is the URI where the user will be redirected to after they sign in.
            string redirectUri = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"];

            // Authority is the URI for the identity provider
            string authority = System.Configuration.ConfigurationManager.AppSettings["AuthorityUri"];

            IFOAuthOptions options = new IFOAuthOptions();
            // from https://identity.intelliflo.com/core/.well-known/openid-configuration
            options.AuthorizationEndpoint = string.Format(CultureInfo.InvariantCulture, "{0}/core/connect/authorize", authority);
            options.TokenEndpoint = string.Format(CultureInfo.InvariantCulture, "{0}/core/connect/token", authority);
            options.UserInfoEndpoint = string.Format(CultureInfo.InvariantCulture, "{0}/core/connect/userinfo", authority);
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.CallbackPath = new PathString(redirectUri); // must be on https

            // compulsory scopes
            options.Scope.Add("openid");
            options.Scope.Add("myprofile");
            options.Scope.Add("profile");

            // for offline access
            // options.Scope.Add("offline_access ");

            // should match what is configured for the client in the portal
            // options.Scope.Add("client_data"); 
            // options.Scope.Add("client_financial_data");
            // options.Scope.Add("firm_data");

            app.Use(typeof(IFOAuthMiddleware), app, options);
        }
    }
}