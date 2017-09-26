# TENANT CLIENT CREDENTIAL FLOW

# tenant id
TENANT_ID="10155"
# space(%20) separated list of scopes
SCOPE="client_data%20firm_data" 
# UTF-8 base 64 encoded clientId:clientSecret
CLIENT_HDR="YXBwLTQ5NzdlNmItdGNmLTUyZTUwZTQwYWVhMTQwMGE5Y2FjMWJjZmNhNzlmNWYyOklTZD1mUUxKfHFTdklTcnRIJS5TViolTGNHZzg2ag==" 

curl -X POST \
  https://identity.intelliflo.com/core/connect/token \
  -H "authorization: Basic $CLIENT_HDR" \
  -H 'cache-control: no-cache' \
  -H 'content-type: application/x-www-form-urlencoded' \
  -d "grant_type=tenant_client_credentials&scope=$SCOPE&tenant_id=$TENANT_ID"

# EXAMPLE result
#{
#	"access_token":"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ik16OTRwU1RsWkUwYmpJTS01NE5TRUJPU3QtdyIsImtpZCI6Ik16OTRwU1RsWkUwYmpJTS01NE5TRUJPU3QtdyJ9.eyJpc3MiOiJodHRwczovL2lkc3J2My5jb20iLCJhdWQiOiJodHRwczovL2lkc3J2My5jb20vcmVzb3VyY2VzIiwiZXhwIjoxNTA2NDQxMzg0LCJuYmYiOjE1MDY0Mzc3ODQsImNsaWVudF9pZCI6ImFwcC00OTc3ZTZiLXRjZi01MmU1MGU0MGFlYTE0MDBhOWNhYzFiY2ZjYTc5ZjVmMiIsInJlYWNoIjoidGVuYW50IiwidGVuYW50X2lkIjoiMTAxNTUiLCJ0ZW5hbnRfZ3VpZCI6ImRjOTU1MTQzLTY4NGQtNDYwZi1iZmJiLWI2YWU3NTc0YjUzYyIsInNjb3BlIjpbImNsaWVudF9kYXRhIiwiZmlybV9kYXRhIl19.dnxIetvw_nzNBxnyPkyutCfGJj1YDQuoA2bpIM1sW_T8Irw0JN9deTdGGUoVVw2djzHVZdEZ1gAJd2ELed_alGZfdlNvY1_IZr1-rHD3wLZIoMbHhIzlpoDmDbTIgROLF_QcU0elEcTsKvzUF9OZRmwo-Js7Ipat75LVZs6Ocy71oaHbWzBHGItYovVv2obfydWxVs3VyESOLgk0v1_TCzp6yXdN5yvApqflU0RtpObXyNOkEZHL01XbTNqx2AGzkKxcFeW4ZntXXtR0z3nF0TcP9S9F7L2lGyKmC5U6cBESyzTYuYEbrm7SQTlqflivUTn5aoVSB_VFqc-DrAjjzQ",
#	"expires_in":3600,
#	"token_type":"Bearer"
#}
