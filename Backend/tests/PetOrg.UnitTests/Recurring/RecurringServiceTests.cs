using PetOrg.Infrastructure.Persistence.Entities;
using PetOrg.Modules.Recurring.Application;
using PetOrg.Modules.Recurring.Contracts;
using PetOrg.Modules.Recurring.Infrastructure;

namespace PetOrg.UnitTests.Recurring;

public sealed class RecurringServiceTests
{
    [Fact]
    public async Task Enroll_then_cancel_transitions_from_active_to_cancelled_and_keeps_dates()
    {
        var repository = new FakeRecurringRepository();
        var service = new RecurringService(repository);
        var donorId = Guid.NewGuid();
        var startedOn = new DateOnly(2026, 6, 1);
        var cancelledOn = new DateOnly(2026, 7, 1);

        var enrolled = await service.EnrollAsync(new EnrollRecurringRequest
        {
            DonorId = donorId,
            Amount = 100000,
            Currency = "cop",
            StartedOn = startedOn,
        }, CancellationToken.None);

        var cancelled = await service.CancelAsync(enrolled.RecurringDonationId, cancelledOn, CancellationToken.None);

        Assert.NotNull(cancelled);
        Assert.Equal("cancelled", cancelled!.Status);
        Assert.Equal(startedOn, cancelled.StartedOn);
        Assert.Equal(cancelledOn, cancelled.CancelledOn);
    }

    [Fact]
    public async Task Mark_payment_failed_sets_status_payment_failed()
    {
        var repository = new FakeRecurringRepository();
        var service = new RecurringService(repository);

        var enrolled = await service.EnrollAsync(new EnrollRecurringRequest
        {
            DonorId = Guid.NewGuid(),
            Amount = 80000,
            Currency = "COP",
            StartedOn = new DateOnly(2026, 6, 1),
        }, CancellationToken.None);

        var failed = await service.MarkPaymentFailedAsync(enrolled.RecurringDonationId, CancellationToken.None);

        Assert.NotNull(failed);
        Assert.Equal("payment-failed", failed!.Status);
    }

    private sealed class FakeRecurringRepository : IRecurringRepository
    {
        private readonly Dictionary<Guid, RecurringDonation> _storage = new();

        public Task AddAsync(RecurringDonation recurringDonation, CancellationToken cancellationToken)
        {
            _storage[recurringDonation.Id] = recurringDonation;
            return Task.CompletedTask;
        }

        public Task<RecurringDonation?> GetAsync(Guid recurringDonationId, CancellationToken cancellationToken)
        {
            _storage.TryGetValue(recurringDonationId, out var recurring);
            return Task.FromResult(recurring);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
