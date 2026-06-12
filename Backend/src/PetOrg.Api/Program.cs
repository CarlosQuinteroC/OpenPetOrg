using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using PetOrg.Api.Security;
using PetOrg.Infrastructure.Persistence;
using PetOrg.Modules.Consent;
using PetOrg.Modules.Consent.Endpoints;
using PetOrg.Modules.Donations;
using PetOrg.Modules.Donations.Endpoints;
using PetOrg.Modules.Reconciliation;
using PetOrg.Modules.Reconciliation.Endpoints;
using PetOrg.Modules.Receipts;
using PetOrg.Modules.Receipts.Endpoints;
using PetOrg.Modules.Recurring;
using PetOrg.Modules.Recurring.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<ManagedIdentityOptions>()
    .BindConfiguration(ManagedIdentityOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var identity = builder.Configuration
            .GetSection(ManagedIdentityOptions.SectionName)
            .Get<ManagedIdentityOptions>() ?? new ManagedIdentityOptions();

        options.Authority = identity.Authority;
        options.Audience = identity.Audience;
        options.RequireHttpsMetadata = identity.RequireHttpsMetadata;
        options.TokenValidationParameters.ValidIssuer = identity.ValidIssuer;
        options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                if (context.Principal?.Identity is not ClaimsIdentity claimsIdentity)
                {
                    return Task.CompletedTask;
                }

                var existingRoleClaims = claimsIdentity.FindAll(ClaimTypes.Role).Select(c => c.Value).ToHashSet(StringComparer.OrdinalIgnoreCase);
                JwtRoleClaimsMapper.Map(claimsIdentity, existingRoleClaims, context.Principal!, identity.RoleClaimType, identity.ResourceAccessClientId);

                return Task.CompletedTask;
            },
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.DonorPolicy, policy =>
        policy.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.DonorRole));

    options.AddPolicy(AuthorizationPolicies.StaffPolicy, policy =>
        policy.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.StaffRole));
});

var connectionString = builder.Configuration.GetConnectionString("Default");
var forceInMemory = builder.Environment.IsEnvironment("Testing");

builder.Services.AddDbContext<PetOrgDbContext>(options =>
{
    if (!forceInMemory && !string.IsNullOrWhiteSpace(connectionString))
    {
        options.UseNpgsql(connectionString);
    }
    else
    {
        options.UseInMemoryDatabase("petorg-dev");
    }
});

builder.Services
    .AddDonationsModule()
    .AddReconciliationModule()
    .AddRecurringModule()
    .AddConsentModule()
    .AddReceiptsModule();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "PetOrg API",
        Version = "v1",
        Description = "Animal Foundation Platform Colombia backend API.",
    });

    options.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Bearer token. Example: Bearer {token}",
    });

    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PetOrg API v1");
    options.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/me", (ClaimsPrincipal user) => Results.Ok(new
{
    user.Identity?.Name,
    roles = user.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray(),
})).RequireAuthorization();

app.MapDonationEndpoints();
app.MapReconciliationEndpoints();
app.MapRecurringEndpoints();
app.MapConsentEndpoints();
app.MapReceiptEndpoints();

app.Run();

public partial class Program;
