using System.ComponentModel.DataAnnotations;

namespace PetOrg.Api.Security;

public sealed class ManagedIdentityOptions
{
    public const string SectionName = "Authentication:ManagedIdentity";

    [Required]
    public string Authority { get; set; } = "https://example-idp/";

    [Required]
    public string Audience { get; set; } = "petorg-api";

    [Required]
    public string Realm { get; set; } = "petorg-dev";

    [Required]
    public string ClientId { get; set; } = "petorg-api";

    public string? ValidIssuer { get; set; }

    public string RoleClaimType { get; set; } = "roles";

    public string ResourceAccessClientId { get; set; } = "petorg-api";

    public bool RequireHttpsMetadata { get; set; } = true;
}
