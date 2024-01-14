using KeycloakExampleServer.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddAuthentication(options => { options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; })
    .AddCookie()
    .AddScheme<AuthenticationSchemeOptions, ReturnStatusCodeAuthenticationHandler>(
        ReturnStatusCodeAuthenticationDefaults.AuthenticationScheme, _ => { })
    .AddOpenIdConnect(options =>
    {
        options.ClientId = builder.Configuration.GetValue<string>("Keycloak:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("Keycloak:ClientSecret");
        options.MetadataAddress = builder.Configuration.GetValue<string>("Keycloak:MetadataAddress");
        options.CallbackPath = "/Api/Auth/Callback";
        options.ResponseType = "code";
        if (builder.Environment.IsDevelopment())
            options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();