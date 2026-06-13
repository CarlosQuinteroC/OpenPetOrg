using PetOrg.Data.Context;

namespace PetOrg.Services.Health;

public sealed class EfCoreDatabaseConnectivityProbe(PetOrgDbContext dbContext) : IDatabaseConnectivityProbe
{
    public Task<bool> CanConnectAsync(CancellationToken cancellationToken)
    {
        return dbContext.Database.CanConnectAsync(cancellationToken);
    }
}
