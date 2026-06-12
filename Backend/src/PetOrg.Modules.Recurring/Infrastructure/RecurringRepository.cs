using Microsoft.EntityFrameworkCore;
using PetOrg.Infrastructure.Persistence;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Modules.Recurring.Infrastructure;

public sealed class RecurringRepository(PetOrgDbContext dbContext) : IRecurringRepository
{
    public Task AddAsync(RecurringDonation recurringDonation, CancellationToken cancellationToken)
    {
        return dbContext.RecurringDonations.AddAsync(recurringDonation, cancellationToken).AsTask();
    }

    public Task<RecurringDonation?> GetAsync(Guid recurringDonationId, CancellationToken cancellationToken)
    {
        return dbContext.RecurringDonations.FirstOrDefaultAsync(x => x.Id == recurringDonationId, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
