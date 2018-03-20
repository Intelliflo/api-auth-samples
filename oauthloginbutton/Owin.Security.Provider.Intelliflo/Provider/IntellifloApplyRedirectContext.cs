using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace Owin.Security.Provider.Intelliflo.Provider
{
    public class IntellifloApplyRedirectContext : BaseContext<IntellifloAuthenticationOptions>
    {
        /// <summary>
        /// Gets the URI used for the redirect operation.
        /// </summary>
        public string RedirectUri { get; }

        /// <summary>
        /// Gets the authentication properties of the challenge
        /// </summary>
        public AuthenticationProperties Properties { get; }

        public IntellifloApplyRedirectContext(IOwinContext context, IntellifloAuthenticationOptions options,
            AuthenticationProperties properties, string redirectUri) 
            : base(context, options)
        {
            RedirectUri = redirectUri;
            Properties = properties;
        }
    }
}
