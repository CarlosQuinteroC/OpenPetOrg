using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using PetOrg.Api.Security;
using PetOrg.Infrastructure.Persistence;

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
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.DonorPolicy, policy =>
        policy.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.DonorRole));

    options.AddPolicy(AuthorizationPolicies.StaffPolicy, policy =>
        policy.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.StaffRole));
});

builder.Services.AddDbContext<PetOrgDbContext>(options =>
    options.UseInMemoryDatabase("petorg-dev"));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/me", (ClaimsPrincipal user) => Results.Ok(new
{
    user.Identity?.Name,
    roles = user.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray(),
})).RequireAuthorization();

app.Run();
