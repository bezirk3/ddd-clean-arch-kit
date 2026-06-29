using Forkfully.Api.Common.Http;
using Forkfully.Api.Common.Mapping;
using Forkfully.Application.Common.Messaging;
using Forkfully.Application.Guests.Commands.CreateGuest;
using Forkfully.Application.Guests.Queries.GetGuest;
using Forkfully.Contracts.Guests;
using Forkfully.Domain.Guest;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Forkfully.Api.Endpoints;

public static class GuestsEndpoints
{
    public static IEndpointRouteBuilder MapGuestEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/guests");

        group.MapPost("", CreateGuest);
        group.MapGet("/{guestId}", GetGuest);

        return routes;
    }

    public static async Task<IResult> CreateGuest(
        CreateGuestRequest request,
        [FromServices] IRequestHandler<CreateGuestCommand, ErrorOr<Guest>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.Match<IResult>(
            guest => TypedResults.Created($"/guests/{guest.Id.Value}", guest.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }

    public static async Task<IResult> GetGuest(
        string guestId,
        [FromServices] IRequestHandler<GetGuestQuery, ErrorOr<Guest>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetGuestQuery(guestId), cancellationToken);

        return result.Match<IResult>(
            guest => TypedResults.Ok(guest.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }
}
