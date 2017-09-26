using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json.Linq;

namespace IF.Samples.OAuth.RefreshToken.Security
{
    public class IFOAuthContext : BaseContext
    {
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }
        public TimeSpan? ExpiresIn { get; set; }
        public ClaimsIdentity Identity { get; set; }
        public AuthenticationProperties Properties { get; set; }
        public JObject User { get; private set; }
        public string Name { get; private set; }

        public IFOAuthContext(IOwinContext context, JObject user, string accessToken,
            string refreshToken, string expires)
            : base(context)
        {
            User = user;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresIn = TimeSpan.FromSeconds(Int32.Parse(expires, CultureInfo.InvariantCulture));

            Name = TryGetValue(user, "name"); 
        }

        private static string TryGetValue(IDictionary<string, JToken> dictionary, string propertyName )
        {
            return dictionary.ContainsKey(propertyName) ? dictionary[propertyName].ToString() : null;
        }
    }
}