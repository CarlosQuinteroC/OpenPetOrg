namespace PetOrg.Infrastructure.Persistence.Entities;

public sealed class TaxReceipt
{
    public Guid Id { get; set; }
    public Guid DonationId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public string Status { get; set; } = "draft";
    public DateTimeOffset IssuedAt { get; set; }

    public Donation Donation { get; set; } = null!;
}
