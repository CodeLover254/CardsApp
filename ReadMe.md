## Cards App
The cards app restful web service demonstrates capabilities to authenticate users with either an Admin or Member role.
Admins have the access to all cards created by members. Members can create, update, fetch and delete cards they own.
JWT Authentication is used as the default authentication scheme. A quick online demo is available at
https://cardsapp-kgwi.onrender.com/swagger. Please note that the page may take as long as 50 seconds to load for the first
time due to limitations set on the free tier of the service. Test credentials are provided below;

**Admin**

Username: **admin@cardsapp.com**

Password: **Password@123**

**Member**

Username: **user@cardsapp.com**

Password: **Password@456**

## Run Locally

In order to build and run locally, please ensure you have **.NET 8.0** SDK installed in your computer. Also ensure that you
have **Postgres** database installed or running in a Docker container.

Clone the project

```bash
  git clone https://github.com/CodeLover254/CardsApp.git
```

Go to the project directory

```bash
  cd CardsApp
```

Populate appsettings.json

```text
  Add the server, user id and password in the ConnectionStrings->Default json element. Ensure these credentials can connect to your postgres DB.
  Add a string of appropriate length (over 40 random characters recommended) in the Jwt->SecretKey json element.
  Populate the UserApps->Admin and UserApps->Member elements with usernames and passwords of your choice. Usernames should be email addresses
```

Restore dependencies

```bash
  dotnet restore
```

Build the application

```bash
  dotnet build --configuration Release
```

Run application

```bash
  cd CardsApp.Api/bin/Release/net8.0
  dotnet CardsApp.Api.dll
```