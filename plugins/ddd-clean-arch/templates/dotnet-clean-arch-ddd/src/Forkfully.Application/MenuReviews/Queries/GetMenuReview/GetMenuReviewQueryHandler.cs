using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.MenuReview;
using Forkfully.Domain.MenuReview.ValueObjects;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.MenuReviews.Queries.GetMenuReview;

public class GetMenuReviewQueryHandler
    : IRequestHandler<GetMenuReviewQuery, ErrorOr<MenuReview>>
{
    private readonly IApplicationDbContext _context;

    public GetMenuReviewQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<MenuReview>> Handle(
        GetMenuReviewQuery query,
        CancellationToken cancellationToken)
    {
        var menuReviewId = MenuReviewId.Create(Guid.Parse(query.MenuReviewId));
        var menuReview = await _context.MenuReviews.SingleOrDefaultAsync(r => r.Id == menuReviewId, cancellationToken);

        return menuReview is null
            ? Error.NotFound("MenuReview.NotFound", "Menu review not found.")
            : menuReview;
    }
}
