using System.ComponentModel.DataAnnotations;

namespace PetOrg.Modules.Donations.Contracts;

public sealed class CreateDonationRequest
{
    [Required]
    public Guid? DonorId { get; init; }

    [Range(typeof(decimal), "0.01", "999999999999")]
    public decimal Amount { get; init; }

    [Required]
    [StringLength(8, MinimumLength = 3)]
    public string Currency { get; init; } = "COP";

    [Required]
    [RegularExpression("^(online|offline)$")]
    public string Channel { get; init; } = "online";

    [StringLength(128)]
    public string? Reference { get; init; }

    [Required]
    public DateTimeOffset? OccurredAt { get; init; }
}
