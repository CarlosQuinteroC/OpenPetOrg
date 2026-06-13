namespace PetOrg.Services.Auth;

public sealed class KeycloakOptions
{
    public const string SectionName = "Authentication:Keycloak";

    public string Authority { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string Realm { get; init; } = string.Empty;

    public bool RequireHttpsMetadata { get; init; } = true;

    public string ResourceAccessClientId { get; init; } = string.Empty;
}
