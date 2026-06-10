namespace PetOrg.Infrastructure.Persistence.Entities;

public sealed class Donation
{
    public Guid Id { get; set; }
    public Guid? DonorId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "COP";
    public string Channel { get; set; } = "online";
    public string ReconciliationStatus { get; set; } = "pending";
    public string? Reference { get; set; }
    public DateTimeOffset OccurredAt { get; set; }

    public ICollection<TaxReceipt> Receipts { get; set; } = [];
}
