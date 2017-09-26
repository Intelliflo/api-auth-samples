using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Owin;

namespace IF.Samples.OAuth.LoginButton.Security
{
    public class IFOAuthMiddleware : AuthenticationMiddleware<IFOAuthOptions>, IDisposable
    {
        private readonly HttpClient httpClient;

        public IFOAuthMiddleware(OwinMiddleware next, IAppBuilder app, IFOAuthOptions options) : base(next, options)
        {
            httpClient = new HttpClient(new WebRequestHandler());
            httpClient.Timeout = TimeSpan.FromSeconds(60); 
            httpClient.MaxResponseContentBufferSize = 1024 * 1024 * 10;

            IDataProtector dataProtecter = app.CreateDataProtector(typeof(IFOAuthMiddleware).FullName, Options.AuthenticationType, "v1");
            Options.StateDataFormat = new PropertiesDataFormat(dataProtecter);
            options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType(); 
        }

        protected override AuthenticationHandler<IFOAuthOptions> CreateHandler()
        {
            return new IFOAuthHandler(httpClient);
        }

        #region IDisposable Support
        private bool isDisposed = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    httpClient.Dispose();
                }

                isDisposed = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}