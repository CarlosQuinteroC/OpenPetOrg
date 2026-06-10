namespace PetOrg.Infrastructure.Persistence.Entities;

public sealed class AnimalCase
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "opened";
    public DateTimeOffset OpenedAt { get; set; }
}
