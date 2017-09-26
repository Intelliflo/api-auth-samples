using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace IF.Samples.OAuth.LoginButton.Security
{
    public class IFOAuthReturnContext : ReturnEndpointContext
    {
        public IFOAuthReturnContext(IOwinContext context, AuthenticationTicket ticket) : base(context, ticket)
        {
        }
    }
}