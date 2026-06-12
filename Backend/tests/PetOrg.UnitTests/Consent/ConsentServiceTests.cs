using PetOrg.Infrastructure.Persistence.Entities;
using PetOrg.Modules.Consent.Application;
using PetOrg.Modules.Consent.Contracts;
using PetOrg.Modules.Consent.Infrastructure;

namespace PetOrg.UnitTests.Consent;

public sealed class ConsentServiceTests
{
    [Fact]
    public async Task Consent_granted_then_revoked_makes_visibility_false_after_revocation()
    {
        var repository = new FakeConsentRepository();
        var service = new ConsentService(repository);
        var donorId = Guid.NewGuid();

        await service.AppendConsentEventAsync(donorId, new UpdatePublicRecognitionConsentRequest
        {
            Granted = true,
            EffectiveAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            ActorId = "staff-01",
            Note = "grant",
        }, CancellationToken.None);

        await service.AppendConsentEventAsync(donorId, new UpdatePublicRecognitionConsentRequest
        {
            Granted = false,
            EffectiveAt = new DateTimeOffset(2026, 2, 1, 0, 0, 0, TimeSpan.Zero),
            ActorId = "staff-01",
            Note = "revoke",
        }, CancellationToken.None);

        var visibility = await service.GetVisibilityAsync(
            donorId,
            new DateTimeOffset(2026, 2, 15, 0, 0, 0, TimeSpan.Zero),
            CancellationToken.None);

        Assert.False(visibility.IsVisible);
    }

    [Fact]
    public async Task Missing_explicit_consent_keeps_donor_hidden()
    {
        var repository = new FakeConsentRepository();
        var service = new ConsentService(repository);

        var visibility = await service.GetVisibilityAsync(Guid.NewGuid(), DateTimeOffset.UtcNow, CancellationToken.None);

        Assert.False(visibility.IsVisible);
        Assert.Null(visibility.SourceEventId);
    }

    private sealed class FakeConsentRepository : IConsentRepository
    {
        private readonly List<DonorConsentEvent> _events = [];

        public Task AddEventAsync(DonorConsentEvent consentEvent, CancellationToken cancellationToken)
        {
            _events.Add(consentEvent);
            return Task.CompletedTask;
        }

        public Task<List<DonorConsentEvent>> GetEventsForDonorAsync(Guid donorId, CancellationToken cancellationToken)
        {
            var events = _events.Where(x => x.DonorId == donorId)
                .OrderBy(x => x.EffectiveAt)
                .ThenBy(x => x.Id)
                .ToList();

            return Task.FromResult(events);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
