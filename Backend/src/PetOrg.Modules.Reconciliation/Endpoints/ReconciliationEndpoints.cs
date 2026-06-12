using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PetOrg.Modules.Reconciliation.Application;
using PetOrg.Modules.Reconciliation.Contracts;

namespace PetOrg.Modules.Reconciliation.Endpoints;

public static class ReconciliationEndpoints
{
    public static IEndpointRouteBuilder MapReconciliationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reconciliation")
            .RequireAuthorization("StaffOnly");

        group.MapPost("/{donationId:guid}/confirm", async (
            Guid donationId,
            ConfirmDonationMatchRequest request,
            ReconciliationService service,
            CancellationToken cancellationToken) =>
        {
            if (request.MatchedDonorId is null)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.MatchedDonorId)] = ["MatchedDonorId is required."],
                });
            }

            var result = await service.ConfirmUniqueMatchAsync(donationId, request.MatchedDonorId.Value, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithSummary("Confirm donation reconciliation")
        .WithDescription("Confirms a unique donor match and marks donation as confirmed.")
        .Produces<ReconciliationResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/{donationId:guid}/ambiguous", async (
            Guid donationId,
            ResolveAmbiguousMatchRequest _request,
            ReconciliationService service,
            CancellationToken cancellationToken) =>
        {
            var result = await service.FlagAmbiguousAsync(donationId, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithSummary("Mark donation as ambiguous")
        .WithDescription("Moves donation to exception state when donor match is ambiguous.")
        .Produces<ReconciliationResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
