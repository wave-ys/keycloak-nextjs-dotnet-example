using System.Text.Encodings.Web;
using KeycloakExampleServer.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeycloakExampleServer.Controllers;

[ApiController]
[Route("/Api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("Login")]
    public async Task Login([FromQuery] string redirect = "/")
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirect
        };
        await HttpContext.ChallengeAsync("OpenIdConnect", properties);
    }

    [HttpGet("Logout")]
    public async Task Logout([FromQuery] string redirect = "/")
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = "/Api/Auth/Logout-Callback?redirect=" + UrlEncoder.Default.Encode(redirect)
        };
        await HttpContext.SignOutAsync("OpenIdConnect", properties);
    }

    [HttpGet("Logout-Callback")]
    public async Task<IActionResult> LogoutCallback([FromQuery] string redirect = "/")
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect(redirect);
    }

    [HttpGet("Profile")]
    [Authorize]
    public IActionResult Profile()
    {
        return Ok(User.GetAppUser());
    }
}