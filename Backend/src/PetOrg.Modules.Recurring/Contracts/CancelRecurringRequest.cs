using System.ComponentModel.DataAnnotations;

namespace PetOrg.Modules.Recurring.Contracts;

public sealed class CancelRecurringRequest
{
    [Required]
    public DateOnly? CancelledOn { get; init; }
}
