using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Provider;

namespace IF.Samples.OAuth.LoginButton.Security
{
    public class IFOAuthProvider
    {
        public IFOAuthProvider()
        {
            OnAuthenticated = context => Task.FromResult<object>(null);
            OnReturnEndpoint = context => Task.FromResult<object>(null);
            OnApplyRedirect = context => context.Response.Redirect(context.RedirectUri);
        }
        
        public Func<IFOAuthContext, Task> OnAuthenticated { get; set; }
        
        public Func<IFOAuthReturnContext, Task> OnReturnEndpoint { get; set; }
        
        public Action<IFOAuthRedirectContext> OnApplyRedirect { get; set; }

        /// <summary>
        /// Called when the user is successfully authenticated
        /// </summary>
        public virtual Task Authenticated(IFOAuthContext context)
        {
            return OnAuthenticated(context);
        }

        /// <summary>
        /// Called before identity is saved in a local cookie and the browser is redirected to the requested URL.
        /// </summary>
        public virtual Task ReturnEndpoint(IFOAuthReturnContext context)
        {
            return OnReturnEndpoint(context);
        }

        /// <summary>
        /// Called when a Challenge causes a redirect to the authorize endpoint
        /// </summary>
        public virtual void ApplyRedirect(IFOAuthRedirectContext context)
        {
            OnApplyRedirect(context);
        }
    }
}