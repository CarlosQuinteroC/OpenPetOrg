using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace PetOrg.IntegrationTests.Donations;

public sealed class DonationEndpointsTests : IClassFixture<Infrastructure.PetOrgApiFactory>
{
    private readonly Infrastructure.PetOrgApiFactory _factory;

    public DonationEndpointsTests(Infrastructure.PetOrgApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Create_online_donation_records_pending_status()
    {
        var client = _factory.CreateClientAsStaff();

        var donorId = Guid.NewGuid();
        var response = await client.PostAsJsonAsync("/api/donations", new
        {
            donorId,
            amount = 70000,
            currency = "COP",
            channel = "online",
            reference = "gw-123",
            occurredAt = DateTimeOffset.UtcNow,
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var payload = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal("pending", payload.RootElement.GetProperty("reconciliationStatus").GetString());
        Assert.True(payload.RootElement.GetProperty("donationId").GetGuid() != Guid.Empty);
    }

    [Fact]
    public async Task Create_offline_donation_without_donor_returns_validation_error()
    {
        var client = _factory.CreateClientAsStaff();

        var response = await client.PostAsJsonAsync("/api/donations", new
        {
            amount = 25000,
            currency = "COP",
            channel = "offline",
            occurredAt = DateTimeOffset.UtcNow,
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        using var payload = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var errors = payload.RootElement.GetProperty("errors");
        Assert.True(errors.TryGetProperty("DonorId", out _));
    }
}
