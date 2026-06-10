namespace PetOrg.Infrastructure.Persistence.Entities;

public sealed class DonorConsentEvent
{
    public Guid Id { get; set; }
    public Guid DonorId { get; set; }
    public bool Granted { get; set; }
    public DateTimeOffset EffectiveAt { get; set; }
    public string ActorId { get; set; } = string.Empty;
    public string? Note { get; set; }
}
