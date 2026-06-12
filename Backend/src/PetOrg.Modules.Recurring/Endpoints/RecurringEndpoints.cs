using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PetOrg.Modules.Recurring.Application;
using PetOrg.Modules.Recurring.Contracts;

namespace PetOrg.Modules.Recurring.Endpoints;

public static class RecurringEndpoints
{
    public static IEndpointRouteBuilder MapRecurringEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/recurring")
            .RequireAuthorization("StaffOnly");

        group.MapPost("/", async (EnrollRecurringRequest request, RecurringService service, CancellationToken cancellationToken) =>
        {
            if (request.DonorId is null)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.DonorId)] = ["DonorId is required."],
                });
            }

            if (request.StartedOn is null)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.StartedOn)] = ["StartedOn is required."],
                });
            }

            var created = await service.EnrollAsync(request, cancellationToken);
            return Results.Created($"/api/recurring/{created.RecurringDonationId}", created);
        })
        .WithSummary("Enroll recurring donation")
        .WithDescription("Enrolls donor into recurring monthly donation lifecycle.")
        .Produces<RecurringDonationDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/{recurringDonationId:guid}/cancel", async (
            Guid recurringDonationId,
            CancelRecurringRequest request,
            RecurringService service,
            CancellationToken cancellationToken) =>
        {
            if (request.CancelledOn is null)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    [nameof(request.CancelledOn)] = ["CancelledOn is required."],
                });
            }

            var result = await service.CancelAsync(recurringDonationId, request.CancelledOn.Value, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithSummary("Cancel recurring donation")
        .WithDescription("Transitions recurring donation from active to cancelled state.")
        .Produces<RecurringDonationDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/{recurringDonationId:guid}/mark-payment-failed", async (
            Guid recurringDonationId,
            RecurringService service,
            CancellationToken cancellationToken) =>
        {
            var result = await service.MarkPaymentFailedAsync(recurringDonationId, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithSummary("Mark recurring payment failed")
        .WithDescription("Marks recurring donation status as payment-failed while retaining audit history.")
        .Produces<RecurringDonationDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
