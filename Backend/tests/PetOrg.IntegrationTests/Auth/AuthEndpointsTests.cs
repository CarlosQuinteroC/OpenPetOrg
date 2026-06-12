using System.Net;
using System.Net.Http.Json;

namespace PetOrg.IntegrationTests.Auth;

public sealed class AuthEndpointsTests : IClassFixture<Infrastructure.PetOrgApiFactory>
{
    private readonly Infrastructure.PetOrgApiFactory _factory;

    public AuthEndpointsTests(Infrastructure.PetOrgApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Protected_endpoint_without_token_returns_unauthorized()
    {
        var client = _factory.CreateClientAsAnonymous();

        var response = await client.GetAsync("/api/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Staff_role_can_access_staff_protected_donation_endpoint()
    {
        var client = _factory.CreateClientAsStaff();

        var response = await client.PostAsJsonAsync("/api/donations", new
        {
            donorId = Guid.NewGuid(),
            amount = 50000,
            currency = "COP",
            channel = "online",
            occurredAt = DateTimeOffset.UtcNow,
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Donor_role_cannot_access_staff_only_endpoint()
    {
        var client = _factory.CreateClientAsDonor();

        var response = await client.PostAsJsonAsync("/api/donations", new
        {
            donorId = Guid.NewGuid(),
            amount = 50000,
            currency = "COP",
            channel = "online",
            occurredAt = DateTimeOffset.UtcNow,
        });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
