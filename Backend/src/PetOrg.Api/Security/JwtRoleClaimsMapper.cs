using System.Security.Claims;
using System.Text.Json;

namespace PetOrg.Api.Security;

internal static class JwtRoleClaimsMapper
{
    public static void Map(
        ClaimsIdentity identity,
        HashSet<string> existingRoles,
        ClaimsPrincipal principal,
        string roleClaimType,
        string resourceAccessClientId)
    {
        AddSimpleRoleClaims(identity, existingRoles, principal, roleClaimType);
        AddRealmRoleClaims(identity, existingRoles, principal);
        AddResourceRoleClaims(identity, existingRoles, principal, resourceAccessClientId);
    }

    private static void AddSimpleRoleClaims(
        ClaimsIdentity identity,
        HashSet<string> existingRoles,
        ClaimsPrincipal principal,
        string roleClaimType)
    {
        foreach (var roleValue in principal.FindAll(roleClaimType).Select(c => c.Value))
        {
            AddRole(identity, existingRoles, roleValue);
        }
    }

    private static void AddRealmRoleClaims(
        ClaimsIdentity identity,
        HashSet<string> existingRoles,
        ClaimsPrincipal principal)
    {
        var realmAccessClaim = principal.FindFirst("realm_access")?.Value;
        if (string.IsNullOrWhiteSpace(realmAccessClaim))
        {
            return;
        }

        try
        {
            using var realmDoc = JsonDocument.Parse(realmAccessClaim);
            if (!realmDoc.RootElement.TryGetProperty("roles", out var rolesElement) || rolesElement.ValueKind != JsonValueKind.Array)
            {
                return;
            }

            foreach (var roleNode in rolesElement.EnumerateArray())
            {
                if (roleNode.ValueKind == JsonValueKind.String)
                {
                    AddRole(identity, existingRoles, roleNode.GetString());
                }
            }
        }
        catch (JsonException)
        {
            // Ignore malformed realm_access claim and continue without role enrichment.
        }
    }

    private static void AddResourceRoleClaims(
        ClaimsIdentity identity,
        HashSet<string> existingRoles,
        ClaimsPrincipal principal,
        string clientId)
    {
        var resourceAccessClaim = principal.FindFirst("resource_access")?.Value;
        if (string.IsNullOrWhiteSpace(resourceAccessClaim) || string.IsNullOrWhiteSpace(clientId))
        {
            return;
        }

        try
        {
            using var resourceDoc = JsonDocument.Parse(resourceAccessClaim);
            if (!resourceDoc.RootElement.TryGetProperty(clientId, out var clientNode))
            {
                return;
            }

            if (!clientNode.TryGetProperty("roles", out var rolesElement) || rolesElement.ValueKind != JsonValueKind.Array)
            {
                return;
            }

            foreach (var roleNode in rolesElement.EnumerateArray())
            {
                if (roleNode.ValueKind == JsonValueKind.String)
                {
                    AddRole(identity, existingRoles, roleNode.GetString());
                }
            }
        }
        catch (JsonException)
        {
            // Ignore malformed resource_access claim and continue without role enrichment.
        }
    }

    private static void AddRole(ClaimsIdentity identity, HashSet<string> existingRoles, string? role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return;
        }

        var normalizedRole = role.Trim();
        if (existingRoles.Add(normalizedRole))
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, normalizedRole));
        }
    }
}
