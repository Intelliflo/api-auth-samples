using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json.Linq;

namespace IF.Samples.OAuth.RefreshToken.Security
{
    public class IFOAuthHandler : AuthenticationHandler<IFOAuthOptions>
    {
        private readonly HttpClient httpClient;

        public IFOAuthHandler(HttpClient client)
        {
            httpClient = client;
        }

        public override async Task<bool> InvokeAsync()
        {
            if (Options.CallbackPath.HasValue && Options.CallbackPath == Request.Path)
            {
                return await InvokeReturnPathAsync();
            }
            return false;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            AuthenticationProperties properties = null;
            try
            {
                string code = null;
                string state = null;

                IReadableStringCollection query = Request.Query;
                IList<string> values = query.GetValues("code");
                code = values[0];

                values = query.GetValues("state");
                if (values != null && values.Count == 1)
                {
                    state = values[0];
                }

                properties = Options.StateDataFormat.Unprotect(state);

                var oauth2Token = await GetOAuthTokenAsync(code);
                var access = new IFOAuthAccess(oauth2Token);
                
                if (string.IsNullOrWhiteSpace(access.AccessToken))
                {
                    return new AuthenticationTicket(null, properties);
                }

                var accountInformation = await GetUserAccountInformation(access.AccessToken);

                var context = new IFOAuthContext(Context, accountInformation, access);
                context.Identity = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, context.Name, ClaimValueTypes.String, Options.AuthenticationType), // TODO need id this back from user info service
                        new Claim(ClaimTypes.Name, context.Name, ClaimValueTypes.String, Options.AuthenticationType)
                    },
                    Options.AuthenticationType,
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                context.Properties = properties;

                access.Persist(Context);

                await Options.Provider.Authenticated(context);

                return new AuthenticationTicket(context.Identity, context.Properties);
            }
            catch (Exception ex)
            {
                // TODO handle exception
                return new AuthenticationTicket(null, properties);
            }
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode != 401)
            {
                return Task.FromResult<object>(null);
            }

            AuthenticationResponseChallenge challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

            if (challenge != null)
            {
                string baseUri = Request.Scheme + Uri.SchemeDelimiter + Request.Host + Request.PathBase;

                string currentUri = baseUri + Request.Path + Request.QueryString;

                string redirectUri = baseUri + Options.CallbackPath;
                
                AuthenticationProperties extra = challenge.Properties;
                if (string.IsNullOrEmpty(extra.RedirectUri))
                {
                    extra.RedirectUri = currentUri;
                }

                string state = Options.StateDataFormat.Protect(extra);

                var authorizationEndpoint = ConstructFullAuthorizationUri(redirectUri);

                var redirectContext = new IFOAuthRedirectContext(Context, Options, extra, authorizationEndpoint);

                Options.Provider.ApplyRedirect(redirectContext);
            }

            return Task.FromResult<object>(null);
        }

        public async Task<bool> InvokeReturnPathAsync()
        {
            AuthenticationTicket model = await AuthenticateAsync();
            if (model == null)
            {
                Response.StatusCode = 500;
                return true;
            }

            var context = new IFOAuthReturnContext(Context, model);
            context.SignInAsAuthenticationType = Options.SignInAsAuthenticationType;
            context.RedirectUri = model.Properties.RedirectUri;
            model.Properties.RedirectUri = null;

            await Options.Provider.ReturnEndpoint(context);

            if (context.SignInAsAuthenticationType != null && context.Identity != null)
            {
                ClaimsIdentity signInIdentity = context.Identity;
                if (!string.Equals(signInIdentity.AuthenticationType, context.SignInAsAuthenticationType, StringComparison.Ordinal))
                {
                    signInIdentity = new ClaimsIdentity(signInIdentity.Claims, context.SignInAsAuthenticationType, signInIdentity.NameClaimType, signInIdentity.RoleClaimType);
                }
                Context.Authentication.SignIn(context.Properties ?? new AuthenticationProperties{IsPersistent = true}, signInIdentity); 
            }

            if (!context.IsRequestCompleted && context.RedirectUri != null)
            {
                if (context.Identity == null)
                {
                    // report that authentication failed
                    context.RedirectUri = WebUtilities.AddQueryString(context.RedirectUri, "error", "access_denied");
                }
                Response.Redirect(context.RedirectUri);
                context.RequestCompleted();
            }

            return context.IsRequestCompleted;
        }

        private string ConstructFullAuthorizationUri(string redirectUri)
        {
            string scope = string.Join(" ", Options.Scope);

            string authorizationEndpoint = string.Format(CultureInfo.InvariantCulture,
                "{0}?client_id={1}&scope={2}&response_type={3}&redirect_uri={4}",
                Options.AuthorizationEndpoint,
                Uri.EscapeDataString(Options.ClientId),
                Uri.EscapeDataString(scope),
                "code",
                Uri.EscapeDataString(redirectUri));
            return authorizationEndpoint;
        }

        private string GenerateRedirectUri()
        {
            string requestPrefix = Request.Scheme + "://" + Request.Host;

            string redirectUri = requestPrefix + RequestPathBase + Options.CallbackPath;
            return redirectUri;
        }

        private async Task<JObject> GetOAuthTokenAsync(string code)
        {
            var tokenRequestParameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", Options.ClientId),
                new KeyValuePair<string, string>("redirect_uri", GenerateRedirectUri()),
                new KeyValuePair<string, string>("client_secret", Options.ClientSecret), 
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
            
            HttpResponseMessage response = await httpClient.PostAsync(Options.TokenEndpoint, requestContent, Request.CallCancelled);
            response.EnsureSuccessStatusCode(); 
            string oauthTokenResponse = await response.Content.ReadAsStringAsync();

            JObject oauth2Token = JObject.Parse(oauthTokenResponse);
            return oauth2Token;
        }

        private async Task<JObject> GetUserAccountInformation(string accessToken)
        {
            var graphRequest = new HttpRequestMessage(HttpMethod.Get, Options.UserInfoEndpoint);
            graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var graphResponse = await httpClient.SendAsync(graphRequest, Request.CallCancelled);
            graphResponse.EnsureSuccessStatusCode();

            string accountString = await graphResponse.Content.ReadAsStringAsync();
            JObject accountInformation = JObject.Parse(accountString);
            return accountInformation;
        }
    }
}