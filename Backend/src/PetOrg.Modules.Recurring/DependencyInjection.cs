using Microsoft.Extensions.DependencyInjection;
using PetOrg.Modules.Recurring.Application;
using PetOrg.Modules.Recurring.Infrastructure;

namespace PetOrg.Modules.Recurring;

public static class DependencyInjection
{
    public static IServiceCollection AddRecurringModule(this IServiceCollection services)
    {
        services.AddScoped<IRecurringRepository, RecurringRepository>();
        services.AddScoped<RecurringService>();
        return services;
    }
}
