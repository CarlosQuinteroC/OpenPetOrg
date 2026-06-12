using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Modules.Recurring.Infrastructure;

public interface IRecurringRepository
{
    Task AddAsync(RecurringDonation recurringDonation, CancellationToken cancellationToken);
    Task<RecurringDonation?> GetAsync(Guid recurringDonationId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
