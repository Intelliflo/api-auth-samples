using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;

namespace IF.Samples.OAuth.RefreshToken.Security
{
    public static class Startup
    {
        /// <summary>
        /// Configure the application to authenticate using IO
        /// </summary>
        public static void ConfigureAuthWithIntelliflo(IAppBuilder app)
        {
            var options = IFOAuthOptions.Construct();

            app.Use(typeof(IFOAuthMiddleware), app, options);
        }
    }
}