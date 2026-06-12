using System.ComponentModel.DataAnnotations;

namespace PetOrg.Modules.Reconciliation.Contracts;

public sealed class ResolveAmbiguousMatchRequest
{
    [Required]
    public Guid? SelectedDonorId { get; init; }

    [StringLength(1024)]
    public string? ResolutionNote { get; init; }
}
