# api-auth-samples
Intelliflo Open API authentication samples

## Authorization Code Flow 

### Sample in CSharp + MVC
[OAuth using a login button](/oauthloginbutton)

#### Changes from standard VS template
 * Brief description in Project_Readme.html
 * A "Log in with Intelliflo" button on the Login view
 * An "IntellifloLogin" method on the AccountController
 * AppSettings in web.config
 * OAuth middleware in the /Security directory
 * Calling Startup.IFOAuth from App_Start.Startup.Auth

#### Dependencies
 * An appropriately configured client in the developer portal
 * This is configured to use the UAT versions of the portal and identity service

#### Todo
 * Remove hacks to work around the inability to change anything in the developer portal


## Obtaining a Refresh Token

### Sample in CSharp + MVC
[OAuth, obtaining a refresh token](/oauthrefreshtoken)

This is based on the OAuth Login Button example
This uses cookies to persist the access token - this is probably not where you would want to store this data

#### Todo
 * Remove hacks to work around the inability to change anything in the developer portal
 * Refactoring to restructure and remove code clones
