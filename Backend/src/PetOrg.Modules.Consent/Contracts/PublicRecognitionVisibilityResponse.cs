namespace PetOrg.Modules.Consent.Contracts;

public sealed record PublicRecognitionVisibilityResponse(
    Guid DonorId,
    bool IsVisible,
    DateTimeOffset DeterminedAt,
    Guid? SourceEventId);
