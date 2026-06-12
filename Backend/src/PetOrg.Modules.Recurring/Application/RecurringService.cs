using PetOrg.Infrastructure.Persistence.Entities;
using PetOrg.Modules.Recurring.Contracts;
using PetOrg.Modules.Recurring.Infrastructure;

namespace PetOrg.Modules.Recurring.Application;

public sealed class RecurringService(IRecurringRepository repository)
{
    public async Task<RecurringDonationDto> EnrollAsync(EnrollRecurringRequest request, CancellationToken cancellationToken)
    {
        var recurring = new RecurringDonation
        {
            Id = Guid.NewGuid(),
            DonorId = request.DonorId!.Value,
            Amount = request.Amount,
            Currency = request.Currency.ToUpperInvariant(),
            Status = "active",
            StartedOn = request.StartedOn!.Value,
            CancelledOn = null,
        };

        await repository.AddAsync(recurring, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return ToDto(recurring);
    }

    public async Task<RecurringDonationDto?> CancelAsync(Guid recurringDonationId, DateOnly cancelledOn, CancellationToken cancellationToken)
    {
        var recurring = await repository.GetAsync(recurringDonationId, cancellationToken);
        if (recurring is null)
        {
            return null;
        }

        recurring.Status = "cancelled";
        recurring.CancelledOn = cancelledOn;

        await repository.SaveChangesAsync(cancellationToken);
        return ToDto(recurring);
    }

    public async Task<RecurringDonationDto?> MarkPaymentFailedAsync(Guid recurringDonationId, CancellationToken cancellationToken)
    {
        var recurring = await repository.GetAsync(recurringDonationId, cancellationToken);
        if (recurring is null)
        {
            return null;
        }

        recurring.Status = "payment-failed";
        await repository.SaveChangesAsync(cancellationToken);
        return ToDto(recurring);
    }

    private static RecurringDonationDto ToDto(RecurringDonation recurring)
    {
        return new RecurringDonationDto(
            recurring.Id,
            recurring.DonorId,
            recurring.Amount,
            recurring.Currency,
            recurring.Status,
            recurring.StartedOn,
            recurring.CancelledOn);
    }
}
