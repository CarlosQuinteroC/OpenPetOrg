using Microsoft.Extensions.DependencyInjection;
using PetOrg.Modules.Donations.Application;
using PetOrg.Modules.Donations.Infrastructure;

namespace PetOrg.Modules.Donations;

public static class DependencyInjection
{
    public static IServiceCollection AddDonationsModule(this IServiceCollection services)
    {
        services.AddScoped<IDonationRepository, DonationRepository>();
        services.AddScoped<DonationService>();
        return services;
    }
}
