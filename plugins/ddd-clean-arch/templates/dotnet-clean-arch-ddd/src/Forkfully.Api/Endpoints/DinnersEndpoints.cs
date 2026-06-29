using Forkfully.Api.Common.Http;
using Forkfully.Api.Common.Mapping;
using Forkfully.Application.Common.Messaging;
using Forkfully.Application.Dinners.Commands.CreateDinner;
using Forkfully.Application.Dinners.Commands.ReserveDinner;
using Forkfully.Application.Dinners.Queries.GetDinner;
using Forkfully.Application.Dinners.Queries.ListDinners;
using Forkfully.Contracts.Dinners;
using Forkfully.Domain.Dinner;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Forkfully.Api.Endpoints;

public static class DinnersEndpoints
{
    public static IEndpointRouteBuilder MapDinnerEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/dinners");

        group.MapPost("", CreateDinner);
        group.MapGet("", ListDinners);
        group.MapGet("/{dinnerId}", GetDinner);
        group.MapPost("/{dinnerId}/reservations", ReserveDinner);

        return routes;
    }

    public static async Task<IResult> CreateDinner(
        CreateDinnerRequest request,
        [FromServices] IRequestHandler<CreateDinnerCommand, ErrorOr<Dinner>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.Match<IResult>(
            dinner => TypedResults.Created($"/dinners/{dinner.Id.Value}", dinner.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }

    public static async Task<IResult> ListDinners(
        [FromServices] IRequestHandler<ListDinnersQuery, ErrorOr<List<Dinner>>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new ListDinnersQuery(), cancellationToken);

        return result.Match<IResult>(
            dinners => TypedResults.Ok(dinners.Select(dinner => dinner.ToResponse()).ToList()),
            errors => ErrorResults.From(httpContext, errors));
    }

    public static async Task<IResult> GetDinner(
        string dinnerId,
        [FromServices] IRequestHandler<GetDinnerQuery, ErrorOr<Dinner>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetDinnerQuery(dinnerId), cancellationToken);

        return result.Match<IResult>(
            dinner => TypedResults.Ok(dinner.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }

    public static async Task<IResult> ReserveDinner(
        string dinnerId,
        ReserveDinnerRequest request,
        [FromServices] IRequestHandler<ReserveDinnerCommand, ErrorOr<Dinner>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new ReserveDinnerCommand(dinnerId, request.GuestId, request.GuestCount);
        var result = await handler.Handle(command, cancellationToken);

        return result.Match<IResult>(
            dinner => TypedResults.Ok(dinner.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }
}
