using PetOrg.Infrastructure.Persistence.Entities;
using PetOrg.Modules.Receipts.Contracts;
using PetOrg.Modules.Receipts.Infrastructure;

namespace PetOrg.Modules.Receipts.Application;

public sealed class IssueFinalReceiptHandler(IReceiptRepository repository)
{
    public async Task<(IssueFinalReceiptResponse? Receipt, ReceiptIssueFailure? Failure)> HandleAsync(
        Guid donationId,
        IssueFinalReceiptRequest request,
        CancellationToken cancellationToken)
    {
        var donation = await repository.GetDonationAsync(donationId, cancellationToken);
        if (donation is null)
        {
            return (null, new ReceiptIssueFailure("donation_not_found", "Donation not found."));
        }

        if (!string.Equals(donation.ReconciliationStatus, "confirmed", StringComparison.OrdinalIgnoreCase))
        {
            return (null, new ReceiptIssueFailure(
                "donation_not_confirmed",
                "Final receipt can be issued only for reconciliation-confirmed donations."));
        }

        if (donation.DonorId is null)
        {
            return (null, new ReceiptIssueFailure("missing_donor", "Donation has no linked donor."));
        }

        var receipt = new TaxReceipt
        {
            Id = Guid.NewGuid(),
            DonationId = donation.Id,
            ReceiptNumber = request.ReceiptNumber,
            Status = "final",
            IssuedAt = DateTimeOffset.UtcNow,
        };

        await repository.AddReceiptAsync(receipt, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return (new IssueFinalReceiptResponse(
            receipt.Id,
            receipt.DonationId,
            receipt.ReceiptNumber,
            receipt.Status,
            receipt.IssuedAt), null);
    }
}
