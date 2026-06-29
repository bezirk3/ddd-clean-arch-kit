using Forkfully.Api.Common.Http;
using Forkfully.Api.Common.Mapping;
using Forkfully.Application.Bills.Queries.GetBill;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Bill;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Forkfully.Api.Endpoints;

public static class BillsEndpoints
{
    public static IEndpointRouteBuilder MapBillEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/bills");

        group.MapGet("/{billId}", GetBill);

        return routes;
    }

    public static async Task<IResult> GetBill(
        string billId,
        [FromServices] IRequestHandler<GetBillQuery, ErrorOr<Bill>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetBillQuery(billId), cancellationToken);

        return result.Match<IResult>(
            bill => TypedResults.Ok(bill.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }
}
