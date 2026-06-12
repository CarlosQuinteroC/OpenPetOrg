namespace PetOrg.Modules.Reconciliation.Contracts;

public sealed record ReconciliationResponse(
    Guid DonationId,
    string ReconciliationStatus,
    Guid? MatchedDonorId);
