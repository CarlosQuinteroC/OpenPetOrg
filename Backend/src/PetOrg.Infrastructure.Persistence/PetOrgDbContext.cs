using Microsoft.EntityFrameworkCore;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Infrastructure.Persistence;

public sealed class PetOrgDbContext(DbContextOptions<PetOrgDbContext> options) : DbContext(options)
{
    public DbSet<Donation> Donations => Set<Donation>();
    public DbSet<RecurringDonation> RecurringDonations => Set<RecurringDonation>();
    public DbSet<DonorConsentEvent> DonorConsentEvents => Set<DonorConsentEvent>();
    public DbSet<TaxReceipt> TaxReceipts => Set<TaxReceipt>();
    public DbSet<AnimalCase> AnimalCases => Set<AnimalCase>();
    public DbSet<DonationTimelineEvent> DonationTimelineEvents => Set<DonationTimelineEvent>();
    public DbSet<AnimalCaseTimelineEvent> AnimalCaseTimelineEvents => Set<AnimalCaseTimelineEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetOrgDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
