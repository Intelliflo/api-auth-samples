using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Web;

namespace IF.Samples.OAuth.RefreshToken.Security
{
    public class IFOAuthConstants
    {
        public const string AuthenticationTypeName = "Intelliflo";
        public const string DateFormat = "yyyyMMdd'T'HHmmssK";

        public class Claims
        {
            public const string AccessToken = "http://identity.intelliflo.com/core/connect/token/claims/access_token";
            public const string RefreshToken = "http://identity.intelliflo.com/core/connect/token/claims/refresh_token";
            public const string ExpiresAt = "http://identity.intelliflo.com/core/connect/token/claims/expires_at";
        }
    }
}