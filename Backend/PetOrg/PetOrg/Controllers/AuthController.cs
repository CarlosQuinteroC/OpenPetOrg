using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PetOrg.Controllers;

[ApiController]
[Route("api")]
public sealed class AuthController : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public ActionResult<AuthIdentityResponse> Me()
    {
        var roles = User.FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .Distinct(StringComparer.Ordinal)
            .Order(StringComparer.Ordinal)
            .ToArray();

        return Ok(new AuthIdentityResponse(
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub"),
            User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("email"),
            roles));
    }
}

public sealed record AuthIdentityResponse(string? Subject, string? Email, IReadOnlyCollection<string> Roles);
