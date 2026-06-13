using System.Security.Claims;
using System.Text.Json;

namespace PetOrg.Services.Auth;

public static class KeycloakRoleClaimsMapper
{
    public static ClaimsPrincipal MapRoles(ClaimsPrincipal principal, string resourceAccessClientId)
    {
        var sourceIdentity = principal.Identity as ClaimsIdentity;
        if (sourceIdentity is null)
        {
            return principal;
        }

        var identity = new ClaimsIdentity(sourceIdentity);
        var existingRoles = identity.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToHashSet(StringComparer.Ordinal);

        foreach (var role in GetRealmRoles(principal).Concat(GetResourceRoles(principal, resourceAccessClientId)).Distinct(StringComparer.Ordinal))
        {
            if (existingRoles.Add(role))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }

        return new ClaimsPrincipal(identity);
    }

    private static IEnumerable<string> GetRealmRoles(ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst("realm_access");
        if (claim is null)
        {
            return [];
        }

        using var document = JsonDocument.Parse(claim.Value);

        if (!document.RootElement.TryGetProperty("roles", out var roles))
        {
            return [];
        }

        return ReadRoles(roles);
    }

    private static IEnumerable<string> GetResourceRoles(ClaimsPrincipal principal, string resourceAccessClientId)
    {
        var claim = principal.FindFirst("resource_access");
        if (claim is null || string.IsNullOrWhiteSpace(resourceAccessClientId))
        {
            return [];
        }

        using var document = JsonDocument.Parse(claim.Value);

        if (!document.RootElement.TryGetProperty(resourceAccessClientId, out var clientAccess)
            || !clientAccess.TryGetProperty("roles", out var roles))
        {
            return [];
        }

        return ReadRoles(roles);
    }

    private static IReadOnlyList<string> ReadRoles(JsonElement roles)
    {
        if (roles.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return roles.EnumerateArray()
            .Where(role => role.ValueKind == JsonValueKind.String)
            .Select(role => role.GetString())
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role!)
            .ToArray();
    }
}
