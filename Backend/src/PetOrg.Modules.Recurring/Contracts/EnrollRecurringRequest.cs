using System.ComponentModel.DataAnnotations;

namespace PetOrg.Modules.Recurring.Contracts;

public sealed class EnrollRecurringRequest
{
    [Required]
    public Guid? DonorId { get; init; }

    [Range(typeof(decimal), "0.01", "999999999999")]
    public decimal Amount { get; init; }

    [Required]
    [StringLength(8, MinimumLength = 3)]
    public string Currency { get; init; } = "COP";

    [Required]
    public DateOnly? StartedOn { get; init; }
}
