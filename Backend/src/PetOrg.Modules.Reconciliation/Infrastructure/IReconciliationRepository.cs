using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Modules.Reconciliation.Infrastructure;

public interface IReconciliationRepository
{
    Task<Donation?> GetDonationAsync(Guid donationId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
