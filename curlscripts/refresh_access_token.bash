#!/bin/bash
# BASIC REFRESH ACCESS TOKEN REQUEST

# space(%20) separated list of scopes
SCOPES="openid%20myprofile%20profile%20offline_access" 
# UTF-8 base 64 encoded clientId:clientSecret
CLIENT_HDR="YXBwLTQ5NzdlNmItdGNmLTUyZTUwZTQwYWVhMTQwMGE5Y2FjMWJjZmNhNzlmNWYyOklTZD1mUUxKfHFTdklTcnRIJS5TViolTGNHZzg2ag==" 
# refresh token from initial authorization call to identity service
REFRESH_TOKEN="c8de93ead73ad1d2acd3c7cfb83e1ea7"

RESPONSE=$(curl -X POST \
  https://identity.intelliflo.com/core/connect/token \
  -H "authorization: Basic $CLIENT_HDR" \
  -H 'cache-control: no-cache' \
  -H 'content-type: application/x-www-form-urlencoded' \
  -d "grant_type=refresh_token&refresh_token=$REFRESH_TOKEN&scope=$SCOPES")

ECHO $RESPONSE

# EXAMPLE REFRESH ACCESS TOKEN RESPONSE
#{
#    "access_token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ik16OTRwU1RsWkUwYmpJTS01NE5TRUJPU3QtdyIsImtpZCI6Ik16OTRwU1RsWkUwYmpJTS01NE5TRUJPU3QtdyJ9.eyJpc3MiOiJodHRwczovL2lkc3J2My5jb20iLCJhdWQiOiJodHRwczovL2lkc3J2My5jb20vcmVzb3VyY2VzIiwiZXhwIjoxNTA2NDE3MTMxLCJuYmYiOjE1MDY0MTM1MzEsImNsaWVudF9pZCI6ImFwcC0yLWFjZi02ODE0NDk4YjRmMzA0NmNkOTYwZGVkMzg4ZTVhZDkxNiIsInNjb3BlIjpbIm9wZW5pZCIsIm15cHJvZmlsZSIsInByb2ZpbGUiLCJvZmZsaW5lX2FjY2VzcyJdLCJzdWIiOiIwMzE1MDI1Zi1lYjE1LTRjODUtYWU0Yi03NWVkM2EwODdiMGMiLCJhdXRoX3RpbWUiOiIxNTA2NDEzMTM2IiwiaWRwIjoiR2l0SHViIiwiZW1haWwiOiJtaWtlLmR1bmNhbkBpbnRlbGxpZmxvLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjoiVHJ1ZSIsIm5hbWUiOiJtaWtlZHVuY2FuSUYiLCJ1c2VybmFtZSI6Im1pa2VkdW5jYW5JRiIsInVybjpnaXRodWI6bmFtZSI6Ik1pa2UgRHVuY2FuIiwidXJuOmdpdGh1Yjp1cmwiOiJodHRwczovL2FwaS5naXRodWIuY29tL3VzZXJzL21pa2VkdW5jYW5JRiIsInVybjpnaXRodWI6b3JnYW5pc2F0aW9ucyI6IkludGVsbGlmbG8iLCJ1cm46Z2l0aHViOnRlYW1zIjoiRGV2ZWxvcGVycyIsImlkZW50aXR5U2VydmVyVXJpIjoiaHR0cHM6Ly91YXQtaWRlbnRpdHkuaW50ZWxsaWZsby5jb20iLCJhbXIiOlsiZXh0ZXJuYWwiXX0.MptJ8RCu15W9lno8cAWjVj68D6pSHhjzy3Ce8xoxnfK2FPFxdXdkLVVb4Mfb-7lJdDqBpCfkEBQJOpmRT58xyaGUCnme6yiIChFYA6bgjPLc9PwbKiUBdQm3vnDPVGH_jMvcHiCX-ihqwm5XEZlgdtVooZNN9bSU0q5YGeCqjCvRsOjTBi3q8_RSGtlGKPCGcDf_OUwpQ87z1A3eD557ZcclUQUchOdk4HqDRQeswNDhwyogmqEmnXest1c0OzUTAan-CPm6Aoak1C1Czg-gwZ6m-usMKQg_JNJMg_tBRstYTWA4oAhZcSJ4FeDTtO9KAanFKghhI2zjtEAkw1Bs0A",
#    "expires_in": 3600,
#    "token_type": "Bearer",
#    "refresh_token": "b27cfff9ac662b486f8a4a1ea2d5a0d0"
#}