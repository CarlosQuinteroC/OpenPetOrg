using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Infrastructure.Persistence.Configurations;

public sealed class DonationTimelineEventConfiguration : IEntityTypeConfiguration<DonationTimelineEvent>
{
    public void Configure(EntityTypeBuilder<DonationTimelineEvent> builder)
    {
        builder.ToTable("donation_timeline_events");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type).HasMaxLength(32).IsRequired();
        builder.Property(x => x.ActorId).HasMaxLength(128);

        builder.HasIndex(x => new { x.DonationId, x.At });
    }
}
