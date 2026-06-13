namespace PetOrg.Services.Health;

public interface IDatabaseConnectivityProbe
{
    Task<bool> CanConnectAsync(CancellationToken cancellationToken);
}
