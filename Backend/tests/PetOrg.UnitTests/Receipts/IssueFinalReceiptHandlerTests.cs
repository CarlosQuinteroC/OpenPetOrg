using PetOrg.Infrastructure.Persistence.Entities;
using PetOrg.Modules.Receipts.Application;
using PetOrg.Modules.Receipts.Contracts;
using PetOrg.Modules.Receipts.Infrastructure;

namespace PetOrg.UnitTests.Receipts;

public sealed class IssueFinalReceiptHandlerTests
{
    [Fact]
    public async Task Receipt_request_before_confirmation_is_blocked()
    {
        var repository = new FakeReceiptRepository(new Donation
        {
            Id = Guid.NewGuid(),
            DonorId = Guid.NewGuid(),
            ReconciliationStatus = "pending",
        });

        var handler = new IssueFinalReceiptHandler(repository);

        var (receipt, failure) = await handler.HandleAsync(repository.DonationId, new IssueFinalReceiptRequest
        {
            ReceiptNumber = "REC-001",
        }, CancellationToken.None);

        Assert.Null(receipt);
        Assert.NotNull(failure);
        Assert.Equal("donation_not_confirmed", failure!.Code);
    }

    [Fact]
    public async Task Confirmed_donation_issues_final_receipt_with_required_fields()
    {
        var donationId = Guid.NewGuid();
        var donorId = Guid.NewGuid();
        var repository = new FakeReceiptRepository(new Donation
        {
            Id = donationId,
            DonorId = donorId,
            ReconciliationStatus = "confirmed",
        });

        var handler = new IssueFinalReceiptHandler(repository);

        var (receipt, failure) = await handler.HandleAsync(donationId, new IssueFinalReceiptRequest
        {
            ReceiptNumber = "REC-002",
        }, CancellationToken.None);

        Assert.Null(failure);
        Assert.NotNull(receipt);
        Assert.Equal(donationId, receipt!.DonationId);
        Assert.Equal("REC-002", receipt.ReceiptNumber);
        Assert.Equal("final", receipt.Status);
    }

    private sealed class FakeReceiptRepository : IReceiptRepository
    {
        private readonly Donation _donation;

        public Guid DonationId => _donation.Id;

        public FakeReceiptRepository(Donation donation)
        {
            _donation = donation;
        }

        public Task<Donation?> GetDonationAsync(Guid donationId, CancellationToken cancellationToken)
        {
            return Task.FromResult(donationId == _donation.Id ? _donation : null);
        }

        public Task AddReceiptAsync(TaxReceipt receipt, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
