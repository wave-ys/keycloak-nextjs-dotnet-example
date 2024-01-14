using KeycloakExampleServer.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = ReturnErrorAuthenticationDefaults.AuthenticationScheme;
        options.DefaultForbidScheme = ReturnErrorAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddScheme<AuthenticationSchemeOptions, ReturnErrorAuthenticationHandler>(
        ReturnErrorAuthenticationDefaults.AuthenticationScheme, _ => { })
    .AddOpenIdConnect(options =>
    {
        options.ClientId = builder.Configuration.GetValue<string>("Keycloak:ClientId") ??
                           "Keycloak Client Id not found";
        options.ClientSecret = builder.Configuration.GetValue<string>("Keycloak:ClientSecret") ??
                               "Keycloak Client Secret not found";
        options.MetadataAddress = builder.Configuration.GetValue<string>("Keycloak:MetadataAddress") ??
                                  "Keycloak Metadata Address not found";

        options.ResponseType = "code";

        // Keycloak will redirect to these pages after signing in or signing out.
        // Don't forget to configure them in Keycloak dashboard.
        options.CallbackPath = "/Api/Auth/Sign-In-Callback";
        options.SignedOutCallbackPath = "/Api/Auth/Sign-Out-Callback";

        // In addition to the access and refresh tokens, this will also save the ID token,
        // which will be automatically attached to the callback URL during sign-out.
        options.SaveTokens = true;

        if (builder.Environment.IsDevelopment())
            options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();