# api-auth-samples
Intelliflo Open API authentication samples

## Authorization Code Flow 

### sample in CSharp + MVC
[OAuth using a login button](/oauthloginbutton/IF.Samples.OAuth.LoginButton.sln)

#### Changes from standard VS template
 * Brief description in Project_Readme.html
 * A "Log in with Intelliflo" button on the Login view
 * An "IntellifloLogin" method on the AccountController
 * AppSettings in web.config
 * OAuth middleware in the /Security directory
 * Calling Startup.IFOAuth from App_Start.Startup.Auth
