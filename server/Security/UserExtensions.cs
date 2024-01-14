using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KeycloakExampleServer.Security;

public static class UserExtensions
{
    public static AppUser? GetAppUser(this ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated != true)
            return null;

        return new AppUser
        {
            ExternalId = user.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value,
            Name = user.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Name).Value,
            EmailVerified = user.Claims.First(claim => claim.Type == "email_verified").Value == "true",
            EmailAddress = user.Claims.First(claim => claim.Type == ClaimTypes.Email).Value,
        };
    }
}

public class AppUser
{
    public string ExternalId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public bool EmailVerified { get; set; }
    public string EmailAddress { get; set; } = default!;
}