using Microsoft.EntityFrameworkCore;
using PetOrg.Infrastructure.Persistence;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Modules.Donations.Infrastructure;

public sealed class DonationRepository(PetOrgDbContext dbContext) : IDonationRepository
{
    public async Task<Donation> AddAsync(Donation donation, CancellationToken cancellationToken)
    {
        await dbContext.Donations.AddAsync(donation, cancellationToken);
        return donation;
    }

    public async Task<Donation?> GetByIdAsync(Guid donationId, CancellationToken cancellationToken)
    {
        return await dbContext.Donations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == donationId, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
