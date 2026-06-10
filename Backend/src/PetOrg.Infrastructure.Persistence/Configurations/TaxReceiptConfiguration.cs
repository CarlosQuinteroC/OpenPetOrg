using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Infrastructure.Persistence.Configurations;

public sealed class TaxReceiptConfiguration : IEntityTypeConfiguration<TaxReceipt>
{
    public void Configure(EntityTypeBuilder<TaxReceipt> builder)
    {
        builder.ToTable("tax_receipts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReceiptNumber).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(16).IsRequired();
    }
}
