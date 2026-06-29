using Forkfully.Api.Common.Http;
using Forkfully.Api.Common.Mapping;
using Forkfully.Application.Common.Messaging;
using Forkfully.Application.Menus.Commands.CreateMenu;
using Forkfully.Application.Menus.Queries.GetMenu;
using Forkfully.Application.Menus.Queries.ListMenus;
using Forkfully.Contracts.Menus;
using Forkfully.Domain.Menu;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Forkfully.Api.Endpoints;

public static class MenusEndpoints
{
    public static IEndpointRouteBuilder MapMenuEndpoints(this IEndpointRouteBuilder routes)
    {
        // Nested under a host; {hostId} is a route value available to each handler.
        var group = routes.MapGroup("/hosts/{hostId}/menus");

        group.MapPost("", CreateMenu);
        group.MapGet("", ListMenus);
        group.MapGet("/{menuId}", GetMenu);

        return routes;
    }

    public static async Task<IResult> CreateMenu(
        string hostId,
        CreateMenuRequest request,
        [FromServices] IRequestHandler<CreateMenuCommand, ErrorOr<Menu>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(hostId), cancellationToken);

        return result.Match<IResult>(
            menu => TypedResults.Created($"/hosts/{hostId}/menus/{menu.Id.Value}", menu.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }

    public static async Task<IResult> ListMenus(
        string hostId,
        [FromServices] IRequestHandler<ListMenusQuery, ErrorOr<List<Menu>>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new ListMenusQuery(hostId), cancellationToken);

        return result.Match<IResult>(
            menus => TypedResults.Ok(menus.Select(menu => menu.ToResponse()).ToList()),
            errors => ErrorResults.From(httpContext, errors));
    }

    public static async Task<IResult> GetMenu(
        string hostId,
        string menuId,
        [FromServices] IRequestHandler<GetMenuQuery, ErrorOr<Menu>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetMenuQuery(hostId, menuId), cancellationToken);

        return result.Match<IResult>(
            menu => TypedResults.Ok(menu.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }
}
