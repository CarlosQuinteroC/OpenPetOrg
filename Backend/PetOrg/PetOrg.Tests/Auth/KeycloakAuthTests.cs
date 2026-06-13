using System.Security.Claims;
using PetOrg.Services.Auth;
using Xunit;

namespace PetOrg.Tests.Auth;

public sealed class KeycloakAuthTests
{
    [Fact]
    public void ValidateReturnsNoErrorsForCompleteDevelopmentKeycloakConfiguration()
    {
        var options = new KeycloakOptions
        {
            Authority = "http://localhost:8080/realms/petorg",
            Audience = "petorg-api",
            Realm = "petorg",
            ResourceAccessClientId = "petorg-api"
        };

        var errors = KeycloakOptionsValidator.Validate(options);

        Assert.Empty(errors);
    }

    [Fact]
    public void ValidateReportsEveryRequiredKeycloakConfigurationValue()
    {
        var options = new KeycloakOptions();

        var errors = KeycloakOptionsValidator.Validate(options);

        Assert.Equal(
            [
                "Authentication:Keycloak:Authority is required.",
                "Authentication:Keycloak:Audience is required.",
                "Authentication:Keycloak:Realm is required.",
                "Authentication:Keycloak:ResourceAccessClientId is required."
            ],
            errors);
    }

    [Fact]
    public void MapRolesAddsRealmRolesAsStandardRoleClaims()
    {
        var principal = CreatePrincipal(
            new Claim("realm_access", "{\"roles\":[\"donor\",\"staff\"]}"));

        var mapped = KeycloakRoleClaimsMapper.MapRoles(principal, "petorg-api");

        Assert.Equal(["donor", "staff"], mapped.FindAll(ClaimTypes.Role).Select(claim => claim.Value).Order());
    }

    [Fact]
    public void MapRolesAddsOnlyConfiguredResourceClientRolesAsStandardRoleClaims()
    {
        var principal = CreatePrincipal(
            new Claim("resource_access", "{\"petorg-api\":{\"roles\":[\"volunteer\"]},\"other-client\":{\"roles\":[\"ignored\"]}}"));

        var mapped = KeycloakRoleClaimsMapper.MapRoles(principal, "petorg-api");

        Assert.Equal(["volunteer"], mapped.FindAll(ClaimTypes.Role).Select(claim => claim.Value));
    }

    private static ClaimsPrincipal CreatePrincipal(params Claim[] claims)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "keycloak"));
    }
}
