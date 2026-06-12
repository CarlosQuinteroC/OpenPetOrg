namespace PetOrg.Modules.Donations.Contracts;

public sealed record CreateDonationResponse(
    Guid DonationId,
    string ReconciliationStatus);
