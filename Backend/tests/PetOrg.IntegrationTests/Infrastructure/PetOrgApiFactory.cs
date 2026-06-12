using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetOrg.Infrastructure.Persistence;

namespace PetOrg.IntegrationTests.Infrastructure;

public sealed class PetOrgApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"petorg-tests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            // Force integration tests to use InMemory and avoid loading relational provider config
            // from user-secrets/environment that may register Npgsql in startup.
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = string.Empty,
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<PetOrgDbContext>>();
            services.AddDbContext<PetOrgDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.Scheme;
                options.DefaultChallengeScheme = TestAuthHandler.Scheme;
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.Scheme,
                _ => { });
        });
    }

    public HttpClient CreateClientAsStaff()
    {
        return CreateClientWithRole("Staff");
    }

    public HttpClient CreateClientAsDonor()
    {
        return CreateClientWithRole("Donor");
    }

    public HttpClient CreateClientAsAnonymous()
    {
        return CreateClient();
    }

    private HttpClient CreateClientWithRole(string role)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(TestAuthHandler.Scheme, role);
        return client;
    }
}

public sealed class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public new const string Scheme = "TestAuth";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var headerValue))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var raw = headerValue.ToString();
        if (!raw.StartsWith($"{Scheme} ", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var role = raw.Substring(Scheme.Length + 1).Trim();
        if (string.IsNullOrWhiteSpace(role))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing role in auth header."));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "integration-user"),
            new Claim(ClaimTypes.Name, "integration-user"),
            new Claim(ClaimTypes.Role, role),
        };

        var identity = new ClaimsIdentity(claims, Scheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
