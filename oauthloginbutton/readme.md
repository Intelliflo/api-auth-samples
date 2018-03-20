# IF.Samples.LoginButton

This example is based on standard ASP.NET MVC website template supporting ASP.NET Identity. The following changes need to be made in order to add support of Intelliflo login button:

1. Copy `Owin.Security.Provider.Intelliflo` project
2. Update ASP.NET Website to reference this project
3. Modify `App_Start\Startup.Auth.cs` file was follows:

```
            app.UseIntellifloAuthentication(new IntellifloAuthenticationOptions
            {
                ClientId = "clientIdGeneratedByAppStore",
                ClientSecret = "clientSecretetGeneratedByAppStore",
            });
```
**IMPORTANT**: You need to use `http://myWebsite/signin-intelliflo` as RedirectUrl. This value can be changed using `IntellifloAuthenticationOptions.CallbackPath` property.

**LINKS**:
1. Developer portal documentation: [https://developer.intelliflo.com/](https://developer.intelliflo.com/)
2. OWIN Identity Providers: [OwinOAuthProviders
](https://github.com/TerribleDev/OwinOAuthProviders/tree/master/src)