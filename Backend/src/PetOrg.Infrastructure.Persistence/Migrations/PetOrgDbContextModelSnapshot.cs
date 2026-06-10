using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetOrg.Infrastructure.Persistence;

namespace PetOrg.Infrastructure.Persistence.Migrations;

[DbContext(typeof(PetOrgDbContext))]
partial class PetOrgDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.8");

        modelBuilder.Entity("PetOrg.Infrastructure.Persistence.Entities.AnimalCase", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd();
            b.Property<string>("Name").HasMaxLength(120);
            b.Property<DateTimeOffset>("OpenedAt");
            b.Property<string>("Status").HasMaxLength(32);
            b.HasKey("Id");
            b.ToTable("animal_cases");
        });

        modelBuilder.Entity("PetOrg.Infrastructure.Persistence.Entities.AnimalCaseTimelineEvent", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd();
            b.Property<Guid>("AnimalCaseId");
            b.Property<string>("ActorId").HasMaxLength(128);
            b.Property<DateTimeOffset>("At");
            b.Property<string>("Type").HasMaxLength(48);
            b.HasKey("Id");
            b.HasIndex("AnimalCaseId", "At");
            b.ToTable("animal_case_timeline_events");
        });

        modelBuilder.Entity("PetOrg.Infrastructure.Persistence.Entities.Donation", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd();
            b.Property<decimal>("Amount").HasPrecision(18, 2);
            b.Property<string>("Channel").HasMaxLength(16);
            b.Property<string>("Currency").HasMaxLength(8);
            b.Property<Guid?>("DonorId");
            b.Property<DateTimeOffset>("OccurredAt");
            b.Property<string>("ReconciliationStatus").HasMaxLength(16);
            b.Property<string?>("Reference").HasMaxLength(128);
            b.HasKey("Id");
            b.ToTable("donations");
        });

        modelBuilder.Entity("PetOrg.Infrastructure.Persistence.Entities.DonationTimelineEvent", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd();
            b.Property<string?>("ActorId").HasMaxLength(128);
            b.Property<DateTimeOffset>("At");
            b.Property<Guid>("DonationId");
            b.Property<string>("Type").HasMaxLength(32);
            b.HasKey("Id");
            b.HasIndex("DonationId", "At");
            b.ToTable("donation_timeline_events");
        });

        modelBuilder.Entity("PetOrg.Infrastructure.Persistence.Entities.DonorConsentEvent", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd();
            b.Property<string>("ActorId").HasMaxLength(128);
            b.Property<Guid>("DonorId");
            b.Property<DateTimeOffset>("EffectiveAt");
            b.Property<bool>("Granted");
            b.Property<string?>("Note").HasMaxLength(1024);
            b.HasKey("Id");
            b.ToTable("donor_consent_events");
        });

        modelBuilder.Entity("PetOrg.Infrastructure.Persistence.Entities.RecurringDonation", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd();
            b.Property<decimal>("Amount").HasPrecision(18, 2);
            b.Property<DateOnly?>("CancelledOn");
            b.Property<string>("Currency").HasMaxLength(8);
            b.Property<Guid>("DonorId");
            b.Property<DateOnly>("StartedOn");
            b.Property<string>("Status").HasMaxLength(24);
            b.HasKey("Id");
            b.ToTable("recurring_donations");
        });

        modelBuilder.Entity("PetOrg.Infrastructure.Persistence.Entities.TaxReceipt", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd();
            b.Property<Guid>("DonationId");
            b.Property<DateTimeOffset>("IssuedAt");
            b.Property<string>("ReceiptNumber").HasMaxLength(64);
            b.Property<string>("Status").HasMaxLength(16);
            b.HasKey("Id");
            b.HasIndex("DonationId");
            b.ToTable("tax_receipts");
        });

        modelBuilder.Entity("PetOrg.Infrastructure.Persistence.Entities.TaxReceipt", b =>
        {
            b.HasOne("PetOrg.Infrastructure.Persistence.Entities.Donation", "Donation")
                .WithMany("Receipts")
                .HasForeignKey("DonationId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.Navigation("Donation");
        });

        modelBuilder.Entity("PetOrg.Infrastructure.Persistence.Entities.Donation", b =>
        {
            b.Navigation("Receipts");
        });
    }
}
