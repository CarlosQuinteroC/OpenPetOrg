using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using PetOrg.Data.Context;
using PetOrg.Services.Auth;
using PetOrg.Services.Health;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var keycloakSection = builder.Configuration.GetSection(KeycloakOptions.SectionName);
var keycloakOptions = keycloakSection.Get<KeycloakOptions>() ?? new KeycloakOptions();
const string DefaultConnectionStringName = "Default";
const string PostgresHealthCheckName = "postgres";
const string ReadinessTag = "ready";
const string LocalFrontendCorsPolicy = "LocalFrontendCors";

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy(LocalFrontendCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddOptions<KeycloakOptions>()
    .Bind(keycloakSection)
    .Validate(options => KeycloakOptionsValidator.Validate(options).Count == 0, "Keycloak authentication configuration is incomplete.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakOptions.Authority;
        options.Audience = keycloakOptions.Audience;
        options.RequireHttpsMetadata = keycloakOptions.RequireHttpsMetadata;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = keycloakOptions.Audience,
            ValidateIssuer = true,
            NameClaimType = "preferred_username",
            RoleClaimType = ClaimTypes.Role
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                if (context.Principal is not null)
                {
                    context.Principal = KeycloakRoleClaimsMapper.MapRoles(
                        context.Principal,
                        keycloakOptions.ResourceAccessClientId);
                }

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddDbContext<PetOrgDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(DefaultConnectionStringName)));
builder.Services.AddScoped<IDatabaseConnectivityProbe, EfCoreDatabaseConnectivityProbe>();
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseReadinessHealthCheck>(PostgresHealthCheckName, tags: [ReadinessTag]);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(LocalFrontendCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = IsReadinessCheck,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.Run();

static bool IsReadinessCheck(HealthCheckRegistration registration)
{
    return registration.Tags.Contains(ReadinessTag);
}

public partial class Program;
