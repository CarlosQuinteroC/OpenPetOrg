using Microsoft.Extensions.DependencyInjection;
using PetOrg.Modules.Reconciliation.Application;
using PetOrg.Modules.Reconciliation.Infrastructure;

namespace PetOrg.Modules.Reconciliation;

public static class DependencyInjection
{
    public static IServiceCollection AddReconciliationModule(this IServiceCollection services)
    {
        services.AddScoped<IReconciliationRepository, ReconciliationRepository>();
        services.AddScoped<ReconciliationService>();
        return services;
    }
}
