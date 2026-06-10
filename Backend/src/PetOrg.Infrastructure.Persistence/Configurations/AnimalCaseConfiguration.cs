using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Infrastructure.Persistence.Configurations;

public sealed class AnimalCaseConfiguration : IEntityTypeConfiguration<AnimalCase>
{
    public void Configure(EntityTypeBuilder<AnimalCase> builder)
    {
        builder.ToTable("animal_cases");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(32).IsRequired();
    }
}
