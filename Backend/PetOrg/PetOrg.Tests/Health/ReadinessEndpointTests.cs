using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PetOrg.Services.Health;
using Xunit;

namespace PetOrg.Tests.Health;

public sealed class ReadinessEndpointTests
{
    [Fact]
    public async Task HealthLiveReturnsHealthyWithoutDatabaseDependency()
    {
        await using var factory = CreateFactory(databaseCanConnect: false);

        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health/live");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task HealthReadyReturnsHealthyWhenPostgresConnectionIsReachable()
    {
        await using var factory = CreateFactory(databaseCanConnect: true);

        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health/ready");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task HealthReadyReturnsServiceUnavailableWhenPostgresConnectionIsUnreachable()
    {
        await using var factory = CreateFactory(databaseCanConnect: false);

        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health/ready");
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Equal("Unhealthy", await response.Content.ReadAsStringAsync());
    }

    private static WebApplicationFactory<Program> CreateFactory(bool databaseCanConnect)
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<IDatabaseConnectivityProbe>();
                    services.AddSingleton<IDatabaseConnectivityProbe>(new StubDatabaseConnectivityProbe(databaseCanConnect));
                });
            });
    }

    private sealed class StubDatabaseConnectivityProbe(bool canConnect) : IDatabaseConnectivityProbe
    {
        public Task<bool> CanConnectAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(canConnect);
        }
    }
}
