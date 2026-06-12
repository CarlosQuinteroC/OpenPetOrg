using Microsoft.EntityFrameworkCore;
using PetOrg.Infrastructure.Persistence;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Modules.Reconciliation.Infrastructure;

public sealed class ReconciliationRepository(PetOrgDbContext dbContext) : IReconciliationRepository
{
    public Task<Donation?> GetDonationAsync(Guid donationId, CancellationToken cancellationToken)
    {
        return dbContext.Donations.FirstOrDefaultAsync(x => x.Id == donationId, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
