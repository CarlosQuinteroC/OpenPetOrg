using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PetOrg.Modules.Donations.Application;
using PetOrg.Modules.Donations.Contracts;

namespace PetOrg.Modules.Donations.Endpoints;

public static class DonationEndpoints
{
    public static IEndpointRouteBuilder MapDonationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/donations")
            .RequireAuthorization("StaffOnly");

        group.MapPost("/", async (CreateDonationRequest request, DonationService service, CancellationToken cancellationToken) =>
        {
            if (request.DonorId is null)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.DonorId)] = ["DonorId is required for online and offline donation registration."],
                });
            }

            if (request.OccurredAt is null)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.OccurredAt)] = ["OccurredAt is required for donation registration."],
                });
            }

            var created = await service.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/donations/{created.DonationId}", created);
        })
        .WithSummary("Create donation")
        .WithDescription("Registers online or offline donations in the unified ledger.")
        .Produces<CreateDonationResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/{donationId:guid}", async (Guid donationId, DonationService service, CancellationToken cancellationToken) =>
        {
            var donation = await service.GetByIdAsync(donationId, cancellationToken);
            return donation is null ? Results.NotFound() : Results.Ok(donation);
        })
        .WithSummary("Get donation by id")
        .WithDescription("Returns donation details if available and linked to a donor.")
        .Produces<DonationDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
