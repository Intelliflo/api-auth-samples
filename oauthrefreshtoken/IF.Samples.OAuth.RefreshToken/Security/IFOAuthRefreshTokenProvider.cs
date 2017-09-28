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
    /// <summary>
    /// Refreshes the access token based on a valid access token
    /// </summary>
    public static class IFOAuthRefreshTokenProvider
    {
        /// <summary>
        /// Requests a new OAuth token from the Authorization service
        /// </summary>
        /// <param name="context">The context to sue for cookie access</param>
        /// <returns>The OAuth token</returns>
        public static async Task<IFOAuthAccess> RefreshAccessAsync(IOwinContext context)
        {
            try
            {
                var authOptions = IFOAuthOptions.Construct();

                string currentRefreshToken = context.Request.Cookies["iflo_refresh_token"];

                if (currentRefreshToken != null)
                {
                    var oauth2Token = await RefreshAccessTokenAsync(currentRefreshToken, authOptions);
                    IFOAuthAccess access = new IFOAuthAccess(oauth2Token);

                    access.Persist(context);

                    return access;
                }
            }
            catch (Exception)
            {
                // TODO handle exceptions
            }

            return null;
        }

        /// <summary>
        /// Requests a new OAuth token from the Authorization service
        /// </summary>
        /// <param name="refreshToken">The refresh token to use</param>
        /// <param name="options">Parameters to use when making the call</param>
        /// <returns>The OAuth token</returns>
        public static async Task<JObject> RefreshAccessTokenAsync(string refreshToken, IFOAuthOptions options)
        {
            string scopes = string.Join(" ", options.Scope);

            var tokenRequestParameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", refreshToken),
                    new KeyValuePair<string, string>("scope", scopes)
                };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var authorizationHeaderValue = BuildAuthorizationHeaderValue(options);

            var refreshTokenRequest = new HttpRequestMessage(HttpMethod.Post, options.TokenEndpoint);
            refreshTokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeaderValue);
            refreshTokenRequest.Content = new FormUrlEncodedContent(tokenRequestParameters);

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.SendAsync(refreshTokenRequest);

            response.EnsureSuccessStatusCode();
            string oauthTokenResponse = await response.Content.ReadAsStringAsync();

            JObject oauth2Token = JObject.Parse(oauthTokenResponse);
            return oauth2Token;
        }

        /// <summary>
        /// Concatenates and encodes client credentials for use in an authorization header
        /// </summary>
        /// <param name="options">Contains the client credentials</param>
        /// <returns>Client credentials for use in an authorization header</returns>
        private static string BuildAuthorizationHeaderValue(IFOAuthOptions options)
        {
            string authorizationCreds = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", options.ClientId, options.ClientSecret);
            authorizationCreds = authorizationCreds.ToUtf8Hex();
            return authorizationCreds;
        }
    }
}