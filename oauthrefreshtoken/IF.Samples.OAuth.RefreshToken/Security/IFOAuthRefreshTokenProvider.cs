using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace IF.Samples.OAuth.RefreshToken.Security
{
    public static class IFOAuthRefreshTokenProvider
    {
        public static async Task<string> RefreshAccessAsync(IOwinContext Context)
        {
            var Options = IFOAuthOptions.Construct();

            string currentRefreshToken = Context.Request.Cookies["iflo_refresh_token"];

            if (currentRefreshToken != null)
            {
                var oauth2Token = await RefreshAccessTokenAsync(currentRefreshToken, Options);
                IFOAuthAccess access = new IFOAuthAccess(oauth2Token);
                
                access.Persist(Context);

                return access.AccessToken;
            }

            return null;
        }

        public static async Task<JObject> RefreshAccessTokenAsync(string refreshToken, IFOAuthOptions Options)
        {
            string scopes = string.Join(" ", Options.Scope);

            var tokenRequestParameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", refreshToken),
                    new KeyValuePair<string, string>("scope", scopes)
                };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var authorizationHeaderValue = BuildAuthorizationHeaderValue(Options);

            var refreshTokenRequest = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            refreshTokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeaderValue);
            refreshTokenRequest.Content = new FormUrlEncodedContent(tokenRequestParameters);

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.SendAsync(refreshTokenRequest);

            response.EnsureSuccessStatusCode();
            string oauthTokenResponse = await response.Content.ReadAsStringAsync();

            JObject oauth2Token = JObject.Parse(oauthTokenResponse);
            return oauth2Token;
        }

        private static string BuildAuthorizationHeaderValue(IFOAuthOptions Options)
        {
            string authorizationCreds = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", Options.ClientId,
                Options.ClientSecret);
            authorizationCreds = authorizationCreds.ToUtf8Hex();
            return authorizationCreds;
        }
    }
}