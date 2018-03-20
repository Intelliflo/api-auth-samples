using System;

namespace Owin.Security.Provider.Intelliflo
{
    public static class IntellifloAuthenticationExtensions
    {
        public static IAppBuilder UseIntellifloAuthentication(this IAppBuilder app,
            IntellifloAuthenticationOptions options)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            app.Use(typeof(IntellifloAuthenticationMiddleware), app, options);

            return app;
        }

        public static IAppBuilder UseIntellifloAuthentication(this IAppBuilder app, string clientId, string clientSecret)
        {
            return app.UseIntellifloAuthentication(new IntellifloAuthenticationOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            });
        }
    }
}