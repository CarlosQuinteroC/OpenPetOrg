using Microsoft.Extensions.DependencyInjection;
using PetOrg.Modules.Consent.Application;
using PetOrg.Modules.Consent.Infrastructure;

namespace PetOrg.Modules.Consent;

public static class DependencyInjection
{
    public static IServiceCollection AddConsentModule(this IServiceCollection services)
    {
        services.AddScoped<IConsentRepository, ConsentRepository>();
        services.AddScoped<ConsentService>();
        return services;
    }
}
