using Microsoft.Extensions.DependencyInjection;
using PetOrg.Modules.Receipts.Application;
using PetOrg.Modules.Receipts.Infrastructure;

namespace PetOrg.Modules.Receipts;

public static class DependencyInjection
{
    public static IServiceCollection AddReceiptsModule(this IServiceCollection services)
    {
        services.AddScoped<IReceiptRepository, ReceiptRepository>();
        services.AddScoped<IssueFinalReceiptHandler>();
        return services;
    }
}
