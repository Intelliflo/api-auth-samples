using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;

namespace IF.Samples.OAuth.RefreshToken.Security
{
    public class IFOAuthOptions : AuthenticationOptions
    {
        /// <summary>
        /// The application client ID assigned by the developer portal.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The client secret generated in the credentials section of the developer portal.
        /// </summary>
        public string ClientSecret { get; set; }
        
        /// <summary>
        /// Relative path of URL to return the user to once they have logged in
        /// </summary>
        /// <remarks>Should match a redirect URL set up in the developer portal</remarks>
        public PathString CallbackPath { get; set; }

        /// <summary>
        /// List of scopes to use when authenticating
        /// </summary>
        public IList<string> Scope { get; private set; }

        /// <summary>
        /// URL to redirect to when using Authorization Code Flow
        /// </summary>
        public string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// URL of service to get an access token from
        /// </summary>
        public string TokenEndpoint { get; set; }

        /// <summary>
        /// URL of service to get user information from
        /// </summary>
        public string UserInfoEndpoint { get; set; }

        /// <summary>
        /// Default authentication type for the application
        /// </summary>
        public string SignInAsAuthenticationType { get; set; }

        public IFOAuthProvider Provider { get; set; }
        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }
        public ICookieManager CookieManager { get; set; }

        public IFOAuthOptions() : base(IFOAuthConstants.AuthenticationTypeName)
        {
            AuthenticationMode = AuthenticationMode.Passive;
            Scope = new List<string>();
            CookieManager = new ChunkingCookieManager();
            Provider = new IFOAuthProvider();
        }

        /// <summary>
        /// Reads settings from config file and creates a prototypical instance of an IFOAuthOptions entity
        /// </summary>
        /// <returns>A fully populated IFOAuthOptions entity</returns>
        public static IFOAuthOptions Construct()
        {
            // Client Id from the Intelliflo developer portal
            string clientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"];

            // API Key from Intelliflo developer portal
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

            options.Scope.Add("offline_access ");

            // should match what is configured for the client in the portal
            // options.Scope.Add("client_data"); 
            // options.Scope.Add("client_financial_data");
            // options.Scope.Add("firm_data");

            return options;
        }
    }
}