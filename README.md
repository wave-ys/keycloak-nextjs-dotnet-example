# Keycloak Example For NextJS + .NET

This project shows a basic example about how to use Keycloak as the identity provider in a NextJS + .NET project.

## Set up Keycloak

1. Run the following command to start Keycloak service.
    ```shell
    docker compose up -d
    ```
2. Visit Keycloak dashboard at [http://localhost:8080/admin/](http://localhost:8080/admin/) and login
   with `admin/admin`.
3. Create a new ream named `my-app`.
4. Create a new OIDC client with Client ID as `keycloak-example-app`.
    1. Enable "Client authentication".
    2. Enable "Standard flow" for "Authentication flow".
    3. Set "Root URL" to `https://localhost:7126`.
    4. Set "Home URL" to `/`.
    5. Set "Valid redirect URIs" to `/Api/Auth/Sign-In-Callback`.
    6. Set "Valid post logout redirect URIs" to `/Api/Auth/Sign-Out-Callback`.
    7. Disable "Front channel logout" and "Backchannel logout session required".
5. In "Ream settings", enable "User registration", "Email as username" and "Login with email".
6. Copy "Client Secret" and save it
   to [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) with key
   as `Keycloak:ClientSecret`.

## Run the project

1. Run the front end.
   ```shell
   pnpm dev
   ```

2. Run the back end.
   ```shell
   dotnet dev-certs https --trust
   dotnet run --launch-profile https
   ```

Now visit [https://localhost:7126/](https://localhost:7126/).

Remember to launch the HTTPS profile because the HTTP profile is likely to not work properly for OIDC.