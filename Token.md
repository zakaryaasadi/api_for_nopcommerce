
# What can you do with Token?

The nopCommerce API lets you do the following with the token resource.

##### you should add customer role "Api Users" for customer

- Go to nopCommerce Administration ->> Customers ->> Customers ->> {{ Any user }} from table ->> press Edit button ->> Customer roles field ->> choose Api user of this user

+ [GET /api/token  
Generator a AccessToken](#get-apicustomers)


## GET /api/token  

Generator a AccessToken


|  GET |  /api/token |
|:---|:---|
|  username |  Set the username or email for the customer  |
|  password |  Set the password for the customer |

<details><summary>Response</summary><p>
         
```json
         HTTP/1.1 200 OK
         
{
    "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOiIxNjA0MDY2Mjc3IiwiZXhwIjoiMTkxOTQyNjI3NyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InpAeC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImQ2NDM2NWVhLTU3NDYtNGUzMS04ZjcwLWMyYmU2ZmI3OTkyMyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJ6QHguY29tIn0.XVpyZzYStfjde5r6v12fKwDhk2qcybjTYtUlkeesvU0",
    "token_type": "Bearer",
    "expires_in": 1919426277,
    "error_description": null
}
```
</p></details>


## Usage

- Just add this line in the header of any Http request. 

```
 Authorization: "Bearer {your access token}
``` 