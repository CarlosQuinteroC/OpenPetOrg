using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Modules.Donations.Infrastructure;

public interface IDonationRepository
{
    Task<Donation> AddAsync(Donation donation, CancellationToken cancellationToken);
    Task<Donation?> GetByIdAsync(Guid donationId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
