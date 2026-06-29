using Forkfully.Api.Common.Http;
using Forkfully.Api.Common.Mapping;
using Forkfully.Application.Common.Messaging;
using Forkfully.Application.Hosts.Commands.CreateHost;
using Forkfully.Application.Hosts.Queries.GetHost;
using Forkfully.Contracts.Hosts;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
// `Host` collides with Microsoft.Extensions.Hosting.Host (web SDK implicit using).
using DomainHost = Forkfully.Domain.Host.Host;

namespace Forkfully.Api.Endpoints;

public static class HostsEndpoints
{
    public static IEndpointRouteBuilder MapHostEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/hosts");

        group.MapPost("", CreateHost);
        group.MapGet("/{hostId}", GetHost);

        return routes;
    }

    public static async Task<IResult> CreateHost(
        CreateHostRequest request,
        [FromServices] IRequestHandler<CreateHostCommand, ErrorOr<DomainHost>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.Match<IResult>(
            host => TypedResults.Created($"/hosts/{host.Id.Value}", host.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }

    public static async Task<IResult> GetHost(
        string hostId,
        [FromServices] IRequestHandler<GetHostQuery, ErrorOr<DomainHost>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetHostQuery(hostId), cancellationToken);

        return result.Match<IResult>(
            host => TypedResults.Ok(host.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }
}
