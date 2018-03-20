using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json.Linq;

namespace Owin.Security.Provider.Intelliflo.Provider
{
    public class IntellifloAuthenticatedContext : BaseContext
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public TimeSpan? ExpiresIn { get; set; }
        public ClaimsIdentity Identity { get; set; }
        public AuthenticationProperties Properties { get; set; }
        public JObject User { get; }
        public string Name { get; }

        public IntellifloAuthenticatedContext(IOwinContext context, JObject user, string accessToken, string refreshToken, string expires) 
            : base(context)
        {
            User = user;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresIn = TimeSpan.FromSeconds(Int32.Parse(expires, CultureInfo.InvariantCulture));

            Name = TryGetValue(user, "name");
        }

        private static string TryGetValue(IDictionary<string, JToken> dictionary, string propertyName)
        {
            return dictionary.ContainsKey(propertyName) ? dictionary[propertyName].ToString() : null;
        }
    }
}
