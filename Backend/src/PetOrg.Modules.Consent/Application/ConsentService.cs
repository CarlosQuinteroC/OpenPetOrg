using PetOrg.Infrastructure.Persistence.Entities;
using PetOrg.Modules.Consent.Contracts;
using PetOrg.Modules.Consent.Infrastructure;

namespace PetOrg.Modules.Consent.Application;

public sealed class ConsentService(IConsentRepository repository)
{
    public async Task<DonorConsentEventDto> AppendConsentEventAsync(
        Guid donorId,
        UpdatePublicRecognitionConsentRequest request,
        CancellationToken cancellationToken)
    {
        var consentEvent = new DonorConsentEvent
        {
            Id = Guid.NewGuid(),
            DonorId = donorId,
            Granted = request.Granted!.Value,
            EffectiveAt = request.EffectiveAt!.Value,
            ActorId = request.ActorId,
            Note = request.Note,
        };

        await repository.AddEventAsync(consentEvent, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return ToDto(consentEvent);
    }

    public async Task<PublicRecognitionVisibilityResponse> GetVisibilityAsync(Guid donorId, DateTimeOffset? at, CancellationToken cancellationToken)
    {
        var snapshotAt = at ?? DateTimeOffset.UtcNow;
        var events = await repository.GetEventsForDonorAsync(donorId, cancellationToken);
        var latestEvent = events
            .Where(x => x.EffectiveAt <= snapshotAt)
            .OrderByDescending(x => x.EffectiveAt)
            .ThenByDescending(x => x.Id)
            .FirstOrDefault();

        return new PublicRecognitionVisibilityResponse(
            donorId,
            latestEvent?.Granted ?? false,
            snapshotAt,
            latestEvent?.Id);
    }

    private static DonorConsentEventDto ToDto(DonorConsentEvent consentEvent)
    {
        return new DonorConsentEventDto(
            consentEvent.Id,
            consentEvent.DonorId,
            consentEvent.Granted,
            consentEvent.EffectiveAt,
            consentEvent.ActorId,
            consentEvent.Note);
    }
}
