# RESOURCE OWNER AUTHENTICATION FLOW

# resource owner username
USERNAME="myusername"
# resource owner password
PASSWORD="secretpassword"
# space(%20) separated list of scopes
SCOPES="openid%20myprofile%20firm_data"
# UTF-8 base 64 encoded clientId:clientSecret
CLIENT_HDR="YXBwLTQ5NzdlNmItdGNmLTUyZTUwZTQwYWVhMTQwMGE5Y2FjMWJjZmNhNzlmNWYyOklTZD1mUUxKfHFTdklTcnRIJS5TViolTGNHZzg2ag==" 

curl -X POST \
  https://identity.intelliflo.com/core/connect/token \
  -H "authorization: Basic $CLIENT_HDR" \
  -H "cache-control: no-cache" \
  -H "content-type: application/x-www-form-urlencoded" \
  -d "grant_type=password&scope=$SCOPES&username=$USERNAME&password=$PASSWORD"  
