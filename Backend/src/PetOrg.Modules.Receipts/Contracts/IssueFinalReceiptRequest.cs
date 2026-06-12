using System.ComponentModel.DataAnnotations;

namespace PetOrg.Modules.Receipts.Contracts;

public sealed class IssueFinalReceiptRequest
{
    [Required]
    [StringLength(64)]
    public string ReceiptNumber { get; init; } = string.Empty;
}
