using PetOrg.Modules.Reconciliation.Contracts;
using PetOrg.Modules.Reconciliation.Infrastructure;

namespace PetOrg.Modules.Reconciliation.Application;

public sealed class ReconciliationService(IReconciliationRepository repository)
{
    public async Task<ReconciliationResponse?> ConfirmUniqueMatchAsync(Guid donationId, Guid matchedDonorId, CancellationToken cancellationToken)
    {
        var donation = await repository.GetDonationAsync(donationId, cancellationToken);
        if (donation is null)
        {
            return null;
        }

        donation.DonorId = matchedDonorId;
        donation.ReconciliationStatus = "confirmed";

        await repository.SaveChangesAsync(cancellationToken);
        return new ReconciliationResponse(donation.Id, donation.ReconciliationStatus, donation.DonorId);
    }

    public async Task<ReconciliationResponse?> FlagAmbiguousAsync(Guid donationId, CancellationToken cancellationToken)
    {
        var donation = await repository.GetDonationAsync(donationId, cancellationToken);
        if (donation is null)
        {
            return null;
        }

        donation.ReconciliationStatus = "exception";
        await repository.SaveChangesAsync(cancellationToken);

        return new ReconciliationResponse(donation.Id, donation.ReconciliationStatus, donation.DonorId);
    }
}
