using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Modules.Consent.Infrastructure;

public interface IConsentRepository
{
    Task AddEventAsync(DonorConsentEvent consentEvent, CancellationToken cancellationToken);
    Task<List<DonorConsentEvent>> GetEventsForDonorAsync(Guid donorId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
