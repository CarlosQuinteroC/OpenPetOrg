using System.ComponentModel.DataAnnotations;

namespace PetOrg.Api.Security;

public sealed class ManagedIdentityOptions
{
    public const string SectionName = "Authentication:ManagedIdentity";

    [Required]
    public string Authority { get; set; } = "https://example-idp/";

    [Required]
    public string Audience { get; set; } = "petorg-api";

    public string? ValidIssuer { get; set; }

    public bool RequireHttpsMetadata { get; set; } = true;
}
