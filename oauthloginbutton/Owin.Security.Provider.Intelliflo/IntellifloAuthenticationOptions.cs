using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Owin.Security.Provider.Intelliflo.Provider;

namespace Owin.Security.Provider.Intelliflo
{
    public class IntellifloAuthenticationOptions : AuthenticationOptions
    {
        /// <summary>
        ///     Gets or sets the a pinned certificate validator to use to validate the endpoints used
        ///     in back channel communications belong to Intelliflo.
        /// </summary>
        /// <value>
        ///     The pinned certificate validator.
        /// </value>
        /// <remarks>
        ///     If this property is null then the default certificate checks are performed,
        ///     validating the subject name and if the signing chain is a trusted party.
        /// </remarks>
        public ICertificateValidator BackchannelCertificateValidator { get; set; }

        /// <summary>
        ///     The HttpMessageHandler used to communicate with Intelliflo.
        ///     This cannot be set at the same time as BackchannelCertificateValidator unless the value
        ///     can be downcast to a WebRequestHandler.
        /// </summary>
        public HttpMessageHandler BackchannelHttpHandler { get; set; }

        /// <summary>
        ///     Gets or sets timeout value in milliseconds for back channel communications with Intelliflo.
        /// </summary>
        /// <value>
        ///     The back channel timeout in milliseconds.
        /// </value>
        public TimeSpan BackchannelTimeout { get; set; }

        /// <summary>
        ///     The request path within the application's base path where the user-agent will be returned.
        ///     The middleware will process this request when it arrives.
        ///     Default value is "/signin-intelliflo".
        /// </summary>
        public PathString CallbackPath { get; set; }

        /// <summary>
        ///     Gets or sets the middleware host name.
        ///     The middleware processes the <see cref="CallbackPath"/> on this host name instead of the application's request host.
        ///     If this is not set, the application's request host will be used.
        /// </summary>
        /// <remarks>
        ///     Use this property when running behind a proxy.
        /// </remarks>
        public string ProxyHost { get; set; }

        /// <summary>
        ///     Get or sets the text that the user can display on a sign in user interface.
        /// </summary>
        public string Caption
        {
            get => Description.Caption;
            set => Description.Caption = value;
        }

        /// <summary>
        ///     Gets or sets the Intelliflo supplied Client ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        ///     Gets or sets the Intelliflo supplied Client Secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IIntellifloAuthenticationProvider" /> used in the authentication events
        /// </summary>
        public IIntellifloAuthenticationProvider Provider { get; set; }

        /// <summary>
        /// A list of permissions to request.
        /// </summary>
        public IList<string> Scope { get; }

        /// <summary>
        /// Base address of identity server
        /// </summary>
        public string IdentityServerBaseUrl { get; set; } = "https://identity.intelliflo.com";

        /// <summary>
        ///     Gets or sets the name of another authentication middleware which will be responsible for actually issuing a user
        ///     <see cref="System.Security.Claims.ClaimsIdentity" />.
        /// </summary>
        public string SignInAsAuthenticationType { get; set; }

        /// <summary>
        ///     Gets or sets the type used to secure data handled by the middleware.
        /// </summary>
        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new <see cref="T:Owin.Security.Provider.Intelliflo.IntellifloAuthenticationOptions" />
        /// </summary>
        public IntellifloAuthenticationOptions()
            : base(Constants.DefaultAuthenticationType)
        {
            Caption = Constants.DefaultAuthenticationType;
            CallbackPath = new PathString("/signin-intelliflo");
            AuthenticationMode = AuthenticationMode.Passive;
            Scope = new List<string>
            {
                "openid",
                "myprofile",
                "profile",

                // for offline access
                // "offline_access",

                // should match what is configured for the client in the portal
                // "client_data",
                // "client_financial_data",
                // "firm_data"
            };

            BackchannelTimeout = TimeSpan.FromSeconds(60);
        }
    }
}
