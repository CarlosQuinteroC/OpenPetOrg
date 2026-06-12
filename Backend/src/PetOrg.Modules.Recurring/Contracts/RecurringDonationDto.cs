namespace PetOrg.Modules.Recurring.Contracts;

public sealed record RecurringDonationDto(
    Guid RecurringDonationId,
    Guid DonorId,
    decimal Amount,
    string Currency,
    string Status,
    DateOnly StartedOn,
    DateOnly? CancelledOn);
