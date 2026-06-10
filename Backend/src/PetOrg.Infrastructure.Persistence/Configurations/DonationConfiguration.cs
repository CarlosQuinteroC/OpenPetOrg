using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Infrastructure.Persistence.Configurations;

public sealed class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> builder)
    {
        builder.ToTable("donations");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.Currency).HasMaxLength(8).IsRequired();
        builder.Property(x => x.Channel).HasMaxLength(16).IsRequired();
        builder.Property(x => x.ReconciliationStatus).HasMaxLength(16).IsRequired();
        builder.Property(x => x.Reference).HasMaxLength(128);

        builder.HasMany(x => x.Receipts)
            .WithOne(x => x.Donation)
            .HasForeignKey(x => x.DonationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
