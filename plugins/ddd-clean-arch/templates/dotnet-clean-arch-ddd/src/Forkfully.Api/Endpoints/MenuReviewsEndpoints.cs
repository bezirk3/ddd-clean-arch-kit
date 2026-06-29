using Forkfully.Api.Common.Http;
using Forkfully.Api.Common.Mapping;
using Forkfully.Application.Common.Messaging;
using Forkfully.Application.MenuReviews.Commands.CreateMenuReview;
using Forkfully.Application.MenuReviews.Queries.GetMenuReview;
using Forkfully.Contracts.MenuReviews;
using Forkfully.Domain.MenuReview;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Forkfully.Api.Endpoints;

public static class MenuReviewsEndpoints
{
    public static IEndpointRouteBuilder MapMenuReviewEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/menu-reviews");

        group.MapPost("", CreateMenuReview);
        group.MapGet("/{menuReviewId}", GetMenuReview);

        return routes;
    }

    public static async Task<IResult> CreateMenuReview(
        CreateMenuReviewRequest request,
        [FromServices] IRequestHandler<CreateMenuReviewCommand, ErrorOr<MenuReview>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        return result.Match<IResult>(
            review => TypedResults.Created($"/menu-reviews/{review.Id.Value}", review.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }

    public static async Task<IResult> GetMenuReview(
        string menuReviewId,
        [FromServices] IRequestHandler<GetMenuReviewQuery, ErrorOr<MenuReview>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetMenuReviewQuery(menuReviewId), cancellationToken);

        return result.Match<IResult>(
            review => TypedResults.Ok(review.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }
}
