namespace PetOrg.Infrastructure.Persistence.Entities;

public sealed class DonationTimelineEvent
{
    public Guid Id { get; set; }
    public Guid DonationId { get; set; }
    public string Type { get; set; } = "created";
    public DateTimeOffset At { get; set; }
    public string? ActorId { get; set; }
}
