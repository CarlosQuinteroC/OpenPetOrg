using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PetOrg.Modules.Consent.Application;
using PetOrg.Modules.Consent.Contracts;

namespace PetOrg.Modules.Consent.Endpoints;

public static class ConsentEndpoints
{
    public static IEndpointRouteBuilder MapConsentEndpoints(this IEndpointRouteBuilder app)
    {
        var staffGroup = app.MapGroup("/api/donors/{donorId:guid}/consent/public-recognition")
            .RequireAuthorization("StaffOnly");

        staffGroup.MapPost("/", async (
            Guid donorId,
            UpdatePublicRecognitionConsentRequest request,
            ConsentService service,
            CancellationToken cancellationToken) =>
        {
            if (request.Granted is null)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.Granted)] = ["Granted is required."],
                });
            }

            if (request.EffectiveAt is null)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.EffectiveAt)] = ["EffectiveAt is required."],
                });
            }

            if (string.IsNullOrWhiteSpace(request.ActorId))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.ActorId)] = ["ActorId is required."],
                });
            }

            var result = await service.AppendConsentEventAsync(donorId, request, cancellationToken);
            return Results.Created($"/api/donors/{donorId}/consent/public-recognition/{result.EventId}", result);
        })
        .WithSummary("Append public-recognition consent event")
        .WithDescription("Creates immutable consent events for public donor recognition visibility.")
        .Produces<DonorConsentEventDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        var visibilityGroup = app.MapGroup("/api/donors/{donorId:guid}/public-recognition")
            .RequireAuthorization("StaffOnly");

        visibilityGroup.MapGet("/visibility", async (
            Guid donorId,
            DateTimeOffset? at,
            ConsentService service,
            CancellationToken cancellationToken) =>
        {
            var result = await service.GetVisibilityAsync(donorId, at, cancellationToken);
            return Results.Ok(result);
        })
        .WithSummary("Get donor public-recognition visibility")
        .WithDescription("Computes consent-controlled visibility at a specific timestamp.")
        .Produces<PublicRecognitionVisibilityResponse>(StatusCodes.Status200OK);

        return app;
    }
}
