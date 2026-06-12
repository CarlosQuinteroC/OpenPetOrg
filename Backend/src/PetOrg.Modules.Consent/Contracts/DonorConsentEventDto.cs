namespace PetOrg.Modules.Consent.Contracts;

public sealed record DonorConsentEventDto(
    Guid EventId,
    Guid DonorId,
    bool Granted,
    DateTimeOffset EffectiveAt,
    string ActorId,
    string? Note);
