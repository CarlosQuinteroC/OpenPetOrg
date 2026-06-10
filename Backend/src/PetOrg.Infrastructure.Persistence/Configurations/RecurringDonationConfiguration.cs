using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Infrastructure.Persistence.Configurations;

public sealed class RecurringDonationConfiguration : IEntityTypeConfiguration<RecurringDonation>
{
    public void Configure(EntityTypeBuilder<RecurringDonation> builder)
    {
        builder.ToTable("recurring_donations");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.Currency).HasMaxLength(8).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(24).IsRequired();
    }
}
