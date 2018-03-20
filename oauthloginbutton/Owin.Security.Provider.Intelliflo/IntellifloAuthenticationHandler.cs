using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Owin.Security.Provider.Intelliflo.Provider;

namespace Owin.Security.Provider.Intelliflo
{
    public class IntellifloAuthenticationHandler : AuthenticationHandler<IntellifloAuthenticationOptions>
    {
        private readonly ILogger logger;
        private readonly HttpClient httpClient;

        public IntellifloAuthenticationHandler(HttpClient httpClient, ILogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            AuthenticationProperties properties = null;

            try
            {
                string code = null;
                string state = null;

                var query = Request.Query;
                var values = query.GetValues("code");
                if (values != null && values.Count == 1)
                {
                    code = values[0];
                }
                values = query.GetValues("state");
                if (values != null && values.Count == 1)
                {
                    state = values[0];
                }

                properties = Options.StateDataFormat.Unprotect(state);
                if (properties == null)
                {
                    return null;
                }

                // OAuth2 10.12 CSRF
                if (!ValidateCorrelationId(properties, logger))
                {
                    return new AuthenticationTicket(null, properties);
                }

                var requestPrefix = Request.Scheme + "://" + this.GetHostName();
                var redirectUri = requestPrefix + Request.PathBase + Options.CallbackPath;

                // Build up the body for the token request
                var body = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("client_id", Options.ClientId),
                    new KeyValuePair<string, string>("client_secret", Options.ClientSecret)
                };

                // Request the token
                var tokenResponse = await httpClient.PostAsync(Options.IdentityServerBaseUrl + "/core/connect/token", new FormUrlEncodedContent(body));
                tokenResponse.EnsureSuccessStatusCode();
                var text = await tokenResponse.Content.ReadAsStringAsync();

                // Deserializes the token response
                dynamic response = JsonConvert.DeserializeObject<dynamic>(text);
                var accessToken = (string)response.access_token;
                var expires = (string)response.expires_in;
                var refreshToken = (string)response.refresh_token;

                if (string.IsNullOrWhiteSpace(accessToken))
                    return new AuthenticationTicket(null, properties);

                var accountInformation = await GetUserAccountInformation(accessToken);

                var context = new IntellifloAuthenticatedContext(Context, accountInformation, accessToken, refreshToken, expires);
                context.Identity = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, context.Name, ClaimValueTypes.String, Options.AuthenticationType),
                        new Claim(ClaimTypes.Name, context.Name, ClaimValueTypes.String, Options.AuthenticationType)
                    },
                    Options.AuthenticationType,
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                context.Properties = properties;

                await Options.Provider.Authenticated(context);

                return new AuthenticationTicket(context.Identity, context.Properties);
            }
            catch (Exception ex)
            {
                logger.WriteError(ex.Message);
            }
            return new AuthenticationTicket(null, properties);
        }

        private async Task<JObject> GetUserAccountInformation(string accessToken)
        {
            var graphRequest = new HttpRequestMessage(HttpMethod.Post, Options.IdentityServerBaseUrl + "/core/connect/userinfo");
            graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var graphResponse = await httpClient.SendAsync(graphRequest, Request.CallCancelled);
            graphResponse.EnsureSuccessStatusCode();

            string accountString = await graphResponse.Content.ReadAsStringAsync();
            JObject accountInformation = JObject.Parse(accountString);
            return accountInformation;
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode != 401)
                return Task.FromResult<object>(null);

            var challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

            if (challenge == null)
                return Task.FromResult<object>(null);

            var baseUri =
                Request.Scheme +
                Uri.SchemeDelimiter +
                GetHostName() +
                Request.PathBase;

            var currentUri =
                baseUri +
                Request.Path +
                Request.QueryString;

            var redirectUri =
                baseUri +
                Options.CallbackPath;

            var properties = challenge.Properties;

            if (string.IsNullOrEmpty(properties.RedirectUri))
                properties.RedirectUri = currentUri;

            // OAuth2 10.12 CSRF
            GenerateCorrelationId(properties);

            // comma separated
            var scope = string.Join(" ", Options.Scope);

            // allow scopes to be specified via the authentication properties for this request, when specified they will already be comma separated
            if (properties.Dictionary.ContainsKey("scope"))
            {
                scope = properties.Dictionary["scope"];
            }

            var state = Options.StateDataFormat.Protect(properties);

            var authorizationEndpoint = ConstructFullAuthorizationUri(redirectUri, scope, state);

            var redirectContext = new IntellifloApplyRedirectContext(
                Context, 
                Options,
                properties, 
                authorizationEndpoint);

            Options.Provider.ApplyRedirect(redirectContext);

            return Task.FromResult<object>(null);
        }

        private string ConstructFullAuthorizationUri(string redirectUri, string scope, string state)
        {
            var authorizationEndpoint =
                 Options.IdentityServerBaseUrl + "/core/connect/authorize" +
                "?response_type=code" +
                "&client_id=" + Uri.EscapeDataString(Options.ClientId) +
                "&redirect_uri=" + Uri.EscapeDataString(redirectUri) +
                "&scope=" + Uri.EscapeDataString(scope) +
                "&state=" + Uri.EscapeDataString(state);

            return authorizationEndpoint;
        }

        public override async Task<bool> InvokeAsync()
        {
            return await InvokeReplyPathAsync();
        }

        private async Task<bool> InvokeReplyPathAsync()
        {
            if (!Options.CallbackPath.HasValue || Options.CallbackPath != Request.Path)
                return false;

            var ticket = await AuthenticateAsync();
            if (ticket == null)
            {
                logger.WriteWarning("Invalid return state, unable to redirect.");
                Response.StatusCode = 500;
                return true;
            }

            var context = new IntellifloReturnEndpointContext(Context, ticket)
            {
                SignInAsAuthenticationType = Options.SignInAsAuthenticationType,
                RedirectUri = ticket.Properties.RedirectUri
            };

            await Options.Provider.ReturnEndpoint(context);

            if (context.SignInAsAuthenticationType != null &&
                context.Identity != null)
            {
                var grantIdentity = context.Identity;
                if (!string.Equals(grantIdentity.AuthenticationType, context.SignInAsAuthenticationType, StringComparison.Ordinal))
                {
                    grantIdentity = new ClaimsIdentity(grantIdentity.Claims, context.SignInAsAuthenticationType, grantIdentity.NameClaimType, grantIdentity.RoleClaimType);
                }
                Context.Authentication.SignIn(context.Properties, grantIdentity);
            }

            if (context.IsRequestCompleted || context.RedirectUri == null) return context.IsRequestCompleted;
            var redirectUri = context.RedirectUri;
            if (context.Identity == null)
            {
                // add a redirect hint that sign-in failed in some way
                redirectUri = WebUtilities.AddQueryString(redirectUri, "error", "access_denied");
            }
            Response.Redirect(redirectUri);
            context.RequestCompleted();

            return context.IsRequestCompleted;
        }

        /// <summary>
        ///     Gets proxy host name from <see cref="IntellifloAuthenticationOptions"/> if it is set.
        ///     If proxy host name is not set, gets application request host name.
        /// </summary>
        /// <returns>Host name.</returns>
        private string GetHostName()
        {
            return string.IsNullOrWhiteSpace(Options.ProxyHost) ? Request.Host.ToString() : Options.ProxyHost;
        }
    }
}