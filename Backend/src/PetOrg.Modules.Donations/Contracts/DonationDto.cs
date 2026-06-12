namespace PetOrg.Modules.Donations.Contracts;

public sealed record DonationDto(
    Guid DonationId,
    Guid DonorId,
    decimal Amount,
    string Currency,
    string Channel,
    string ReconciliationStatus,
    string? Reference,
    DateTimeOffset OccurredAt);
