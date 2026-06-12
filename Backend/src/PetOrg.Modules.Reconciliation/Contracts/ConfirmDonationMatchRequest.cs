using System.ComponentModel.DataAnnotations;

namespace PetOrg.Modules.Reconciliation.Contracts;

public sealed class ConfirmDonationMatchRequest
{
    [Required]
    public Guid? MatchedDonorId { get; init; }

    [StringLength(1024)]
    public string? EvidenceNote { get; init; }
}
