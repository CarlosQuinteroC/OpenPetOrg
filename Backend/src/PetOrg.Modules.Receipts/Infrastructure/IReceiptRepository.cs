using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Modules.Receipts.Infrastructure;

public interface IReceiptRepository
{
    Task<Donation?> GetDonationAsync(Guid donationId, CancellationToken cancellationToken);
    Task AddReceiptAsync(TaxReceipt receipt, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
