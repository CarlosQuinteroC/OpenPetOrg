namespace PetOrg.Infrastructure.Persistence.Entities;

public sealed class AnimalCaseTimelineEvent
{
    public Guid Id { get; set; }
    public Guid AnimalCaseId { get; set; }
    public string Type { get; set; } = "opened";
    public DateTimeOffset At { get; set; }
    public string ActorId { get; set; } = string.Empty;
}
