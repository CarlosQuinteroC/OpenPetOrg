using PetOrg.Infrastructure.Persistence.Entities;
using PetOrg.Modules.Donations.Contracts;
using PetOrg.Modules.Donations.Infrastructure;

namespace PetOrg.Modules.Donations.Application;

public sealed class DonationService(IDonationRepository donationRepository)
{
    public async Task<CreateDonationResponse> CreateAsync(CreateDonationRequest request, CancellationToken cancellationToken)
    {
        var donation = new Donation
        {
            Id = Guid.NewGuid(),
            DonorId = request.DonorId,
            Amount = request.Amount,
            Currency = request.Currency.ToUpperInvariant(),
            Channel = request.Channel,
            ReconciliationStatus = "pending",
            Reference = request.Reference,
            OccurredAt = request.OccurredAt!.Value,
        };

        await donationRepository.AddAsync(donation, cancellationToken);
        await donationRepository.SaveChangesAsync(cancellationToken);

        return new CreateDonationResponse(donation.Id, donation.ReconciliationStatus);
    }

    public async Task<DonationDto?> GetByIdAsync(Guid donationId, CancellationToken cancellationToken)
    {
        var donation = await donationRepository.GetByIdAsync(donationId, cancellationToken);
        if (donation is null || donation.DonorId is null)
        {
            return null;
        }

        return new DonationDto(
            donation.Id,
            donation.DonorId.Value,
            donation.Amount,
            donation.Currency,
            donation.Channel,
            donation.ReconciliationStatus,
            donation.Reference,
            donation.OccurredAt);
    }
}
