using System.ComponentModel.DataAnnotations;

namespace PetOrg.Modules.Consent.Contracts;

public sealed class UpdatePublicRecognitionConsentRequest
{
    [Required]
    public bool? Granted { get; init; }

    [Required]
    public DateTimeOffset? EffectiveAt { get; init; }

    [Required]
    [StringLength(128)]
    public string ActorId { get; init; } = string.Empty;

    [StringLength(1024)]
    public string? Note { get; init; }
}
