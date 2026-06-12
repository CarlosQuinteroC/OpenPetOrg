using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace PetOrg.IntegrationTests.Reconciliation;

public sealed class ReconciliationEndpointsTests : IClassFixture<Infrastructure.PetOrgApiFactory>
{
    private readonly Infrastructure.PetOrgApiFactory _factory;

    public ReconciliationEndpointsTests(Infrastructure.PetOrgApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Confirm_endpoint_sets_status_confirmed_and_links_donor()
    {
        var client = _factory.CreateClientAsStaff();
        var donorId = Guid.NewGuid();

        var createDonationResponse = await client.PostAsJsonAsync("/api/donations", new
        {
            donorId,
            amount = 90000,
            currency = "COP",
            channel = "online",
            occurredAt = DateTimeOffset.UtcNow,
        });

        createDonationResponse.EnsureSuccessStatusCode();
        var createPayload = await ReadJson(createDonationResponse);
        var donationId = createPayload.GetProperty("donationId").GetGuid();

        var confirmResponse = await client.PostAsJsonAsync($"/api/reconciliation/{donationId}/confirm", new
        {
            matchedDonorId = donorId,
            evidenceNote = "bank statement match",
        });

        Assert.Equal(HttpStatusCode.OK, confirmResponse.StatusCode);
        var confirmPayload = await ReadJson(confirmResponse);
        Assert.Equal("confirmed", confirmPayload.GetProperty("reconciliationStatus").GetString());
        Assert.Equal(donorId, confirmPayload.GetProperty("matchedDonorId").GetGuid());
    }

    [Fact]
    public async Task Ambiguous_endpoint_sets_status_exception_and_does_not_auto_confirm()
    {
        var client = _factory.CreateClientAsStaff();
        var donorId = Guid.NewGuid();

        var createDonationResponse = await client.PostAsJsonAsync("/api/donations", new
        {
            donorId,
            amount = 120000,
            currency = "COP",
            channel = "offline",
            reference = "cash-receipt-19",
            occurredAt = DateTimeOffset.UtcNow,
        });

        createDonationResponse.EnsureSuccessStatusCode();
        var createPayload = await ReadJson(createDonationResponse);
        var donationId = createPayload.GetProperty("donationId").GetGuid();

        var ambiguousResponse = await client.PostAsJsonAsync($"/api/reconciliation/{donationId}/ambiguous", new
        {
            selectedDonorId = Guid.NewGuid(),
            resolutionNote = "multiple plausible donors",
        });

        Assert.Equal(HttpStatusCode.OK, ambiguousResponse.StatusCode);
        var ambiguousPayload = await ReadJson(ambiguousResponse);
        Assert.Equal("exception", ambiguousPayload.GetProperty("reconciliationStatus").GetString());
    }

    private static async Task<JsonElement> ReadJson(HttpResponseMessage response)
    {
        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        return document.RootElement.Clone();
    }
}
