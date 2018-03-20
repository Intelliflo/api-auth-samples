# api-auth-samples
Intelliflo Open API authentication samples

## Authorization Code Flow 

### Sample in CSharp + MVC
[OAuth using a login button](/oauthloginbutton)

#### Changes from standard VS template
 * OWIN Intelliflo OAuth middleware added as separate project
 * `Startup.Auth.cs` updated to register middleware

#### Pre-requisites
 * An appropriately configured client in the developer portal
 * This is configured to use the UAT versions of the portal and identity service

#### Todo
 * Remove hacks to work around the inability to change anything in the developer portal


## Obtaining a Refresh Token

### Sample in CSharp + MVC
[OAuth, obtaining a refresh token](/oauthrefreshtoken)

This is based on the CSharp Authorization Code Flow sample.
This uses cookies to persist the access token - this is probably not where you would want to store this data.

#### Todo
 * Remove hacks to work around the inability to change anything in the developer portal
 

## cURL Scripts

There are currently examples for:
 * Tenant client credential authorization flow
 * Resource owner authorization flow
 * Requesting a refresh token for offline access
 * Using a refresh token to request an access token

[Scripts](/curlscripts)

#### Pre-requisites
 * An appropriately configured client in the developer portal