using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Infrastructure.Persistence.Configurations;

public sealed class AnimalCaseTimelineEventConfiguration : IEntityTypeConfiguration<AnimalCaseTimelineEvent>
{
    public void Configure(EntityTypeBuilder<AnimalCaseTimelineEvent> builder)
    {
        builder.ToTable("animal_case_timeline_events");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type).HasMaxLength(48).IsRequired();
        builder.Property(x => x.ActorId).HasMaxLength(128).IsRequired();

        builder.HasIndex(x => new { x.AnimalCaseId, x.At });
    }
}
