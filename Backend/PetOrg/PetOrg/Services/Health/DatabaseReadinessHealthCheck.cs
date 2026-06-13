using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PetOrg.Services.Health;

public sealed class DatabaseReadinessHealthCheck(IDatabaseConnectivityProbe databaseConnectivityProbe) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var canConnect = await databaseConnectivityProbe.CanConnectAsync(cancellationToken);

        return canConnect
            ? HealthCheckResult.Healthy("PostgreSQL is reachable.")
            : HealthCheckResult.Unhealthy("PostgreSQL is unreachable.");
    }
}
