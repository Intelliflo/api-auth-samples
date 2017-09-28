#!/bin/bash
# BASIC OFFLINE ACCESS TOKEN REQUEST FOR TENANT CLIENT CREDENTIAL FLOW

# space(%20) separated list of scopes
SCOPES="openid%20myprofile%20profile%20offline_access"
# UTF-8 base 64 encoded clientId:clientSecret
CLIENT_HDR="YXBwLTQ5NzdlNmItdGNmLTUyZTUwZTQwYWVhMTQwMGE5Y2FjMWJjZmNhNzlmNWYyOklTZD1mUUxKfHFTdklTcnRIJS5TViolTGNHZzg2ag==" 
# tenant id
TENANT_ID="10155"

RESPONSE=$(curl -X POST \
  https://identity.intelliflo.com/core/connect/token \
  -H "authorization: Basic $CLIENT_HDR" \
  -H "cache-control: no-cache" \
  -H "content-type: application/x-www-form-urlencoded" \
  -d "grant_type=tenant_client_credentials&tenant_id=$TENANT_ID&scope=$SCOPES")

ECHO $RESPONSE


# BASIC OFFLINE ACCESS TOKEN REQUEST FOR AUTHORIZATION CODE FLOW

# space(%20) separated list of scopes
SCOPES="openid%20myprofile%20profile%20offline_access"
# URI encoded URL back to your application
REDIRECT_URL="https%3A%2F%2Flocalhost%3A44339%2FHome"
# id for client registered in the developer portal
CLIENT_ID="app-2-acf-6814498b4f3046cd960ded388e5ad916"

RESPONSE=$(curl -X GET \
  "https://identity.intelliflo.com/core/connect/authorize?client_id=$CLIENT_ID&response_type=code&scope=$SCOPES&redirect_uri=$REDIRECT_URL" \
  -H "cache-control: no-cache")

ECHO $RESPONSE


# NB: In both cases the client must be configured in the Intelliflo developer portal to have the 'offline_access' scope
