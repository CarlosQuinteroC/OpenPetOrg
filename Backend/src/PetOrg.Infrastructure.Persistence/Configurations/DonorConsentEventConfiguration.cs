using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Infrastructure.Persistence.Configurations;

public sealed class DonorConsentEventConfiguration : IEntityTypeConfiguration<DonorConsentEvent>
{
    public void Configure(EntityTypeBuilder<DonorConsentEvent> builder)
    {
        builder.ToTable("donor_consent_events");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ActorId).HasMaxLength(128).IsRequired();
        builder.Property(x => x.Note).HasMaxLength(1024);
    }
}
