using System;
using System.Collections.Generic;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;

namespace IF.Samples.OAuth.LoginButton.Security
{
    public class IFOAuthOptions : AuthenticationOptions
    {
        internal const string AuthenticationTypeName = "Intelliflo";

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


        public IList<string> Scope { get; private set; }
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string UserInfoEndpoint { get; set; }

        public string SignInAsAuthenticationType { get; set; }
        public IFOAuthProvider Provider { get; set; }
        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }
        public ICookieManager CookieManager { get; set; }

        public IFOAuthOptions() : base(AuthenticationTypeName)
        {
            AuthenticationMode = AuthenticationMode.Passive;
            Scope = new List<string>();
            CookieManager = new CookieManager();
            Provider = new IFOAuthProvider();
        }
    }
}