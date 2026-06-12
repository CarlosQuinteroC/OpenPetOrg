using Microsoft.EntityFrameworkCore;
using PetOrg.Infrastructure.Persistence;
using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.Modules.Receipts.Infrastructure;

public sealed class ReceiptRepository(PetOrgDbContext dbContext) : IReceiptRepository
{
    public Task<Donation?> GetDonationAsync(Guid donationId, CancellationToken cancellationToken)
    {
        return dbContext.Donations.FirstOrDefaultAsync(x => x.Id == donationId, cancellationToken);
    }

    public Task AddReceiptAsync(TaxReceipt receipt, CancellationToken cancellationToken)
    {
        return dbContext.TaxReceipts.AddAsync(receipt, cancellationToken).AsTask();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
