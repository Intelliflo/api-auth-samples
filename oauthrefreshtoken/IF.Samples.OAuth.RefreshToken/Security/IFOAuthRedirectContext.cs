using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace IF.Samples.OAuth.RefreshToken.Security
{
    public class IFOAuthRedirectContext : BaseContext<IFOAuthOptions>
    {
        public IFOAuthRedirectContext(IOwinContext context, IFOAuthOptions options,
            AuthenticationProperties authProperties, string redirectUri) : base(context, options)
        {
            RedirectUri = redirectUri;
            Properties = authProperties;
        }

        public string RedirectUri { get; private set; }

        /// <summary>
        /// Gets the authenticaiton properties of the challenge
        /// </summary>
        public AuthenticationProperties Properties { get; private set; }
    }
}