using Microsoft.EntityFrameworkCore;
using PetOrg.Infrastructure.Persistence;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Modules.Consent.Infrastructure;

public sealed class ConsentRepository(PetOrgDbContext dbContext) : IConsentRepository
{
    public Task AddEventAsync(DonorConsentEvent consentEvent, CancellationToken cancellationToken)
    {
        return dbContext.DonorConsentEvents.AddAsync(consentEvent, cancellationToken).AsTask();
    }

    public Task<List<DonorConsentEvent>> GetEventsForDonorAsync(Guid donorId, CancellationToken cancellationToken)
    {
        return dbContext.DonorConsentEvents
            .AsNoTracking()
            .Where(x => x.DonorId == donorId)
            .OrderBy(x => x.EffectiveAt)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
