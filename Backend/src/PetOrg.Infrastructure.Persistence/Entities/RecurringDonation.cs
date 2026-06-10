namespace PetOrg.Infrastructure.Persistence.Entities;

public sealed class RecurringDonation
{
    public Guid Id { get; set; }
    public Guid DonorId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "COP";
    public string Status { get; set; } = "active";
    public DateOnly StartedOn { get; set; }
    public DateOnly? CancelledOn { get; set; }
}
