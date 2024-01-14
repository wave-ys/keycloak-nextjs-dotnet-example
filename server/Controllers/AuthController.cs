using KeycloakExampleServer.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeycloakExampleServer.Controllers;

[ApiController]
[Route("/Api/[controller]")]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    private readonly string _keycloakClientId =
        configuration.GetValue<string>("Keycloak:ClientId") ?? "Keycloak Client Id not found";

    [HttpGet("Login")]
    public async Task Login([FromQuery] string? redirect = "/")
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirect
        };
        await HttpContext.ChallengeAsync("OpenIdConnect", properties);
    }

    [HttpGet("Logout")]
    [Authorize]
    public async Task Logout([FromQuery] string? redirect = "/")
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirect
        };
        properties.SetParameter("client_id", _keycloakClientId);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync("OpenIdConnect", properties);
    }

    [HttpGet("Profile")]
    [Authorize]
    public IActionResult Profile()
    {
        return Ok(User.GetAppUser());
    }
}