using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PetOrg.Modules.Receipts.Application;
using PetOrg.Modules.Receipts.Contracts;

namespace PetOrg.Modules.Receipts.Endpoints;

public static class ReceiptEndpoints
{
    public static IEndpointRouteBuilder MapReceiptEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/receipts")
            .RequireAuthorization("StaffOnly");

        group.MapPost("/{donationId:guid}/issue-final", async (
            Guid donationId,
            IssueFinalReceiptRequest request,
            IssueFinalReceiptHandler handler,
            CancellationToken cancellationToken) =>
        {
            if (string.IsNullOrWhiteSpace(request.ReceiptNumber))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.ReceiptNumber)] = ["ReceiptNumber is required."],
                });
            }

            var (receipt, failure) = await handler.HandleAsync(donationId, request, cancellationToken);
            if (failure is not null)
            {
                return failure.Code switch
                {
                    "donation_not_found" => Results.NotFound(failure),
                    "donation_not_confirmed" => Results.Conflict(failure),
                    "missing_donor" => Results.UnprocessableEntity(failure),
                    _ => Results.BadRequest(failure),
                };
            }

            return Results.Created($"/api/receipts/{receipt!.ReceiptId}", receipt);
        })
        .WithSummary("Issue final receipt")
        .WithDescription("Issues final receipt only for reconciliation-confirmed donations.")
        .Produces<IssueFinalReceiptResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .Produces<ReceiptIssueFailure>(StatusCodes.Status404NotFound)
        .Produces<ReceiptIssueFailure>(StatusCodes.Status409Conflict)
        .Produces<ReceiptIssueFailure>(StatusCodes.Status422UnprocessableEntity);

        return app;
    }
}
