using Forkfully.Application.MenuReviews.Commands.CreateMenuReview;
using Forkfully.Contracts.MenuReviews;
using DomainMenuReview = Forkfully.Domain.MenuReview.MenuReview;

namespace Forkfully.Api.Common.Mapping;

public static class MenuReviewMappings
{
    public static CreateMenuReviewCommand ToCommand(this CreateMenuReviewRequest request) =>
        new(
            request.HostId,
            request.MenuId,
            request.GuestId,
            request.DinnerId,
            request.Rating,
            request.Comment);

    public static MenuReviewResponse ToResponse(this DomainMenuReview review) =>
        new(
            review.Id.Value.ToString(),
            review.Rating,
            review.Comment,
            review.HostId.Value.ToString(),
            review.MenuId.Value.ToString(),
            review.GuestId.Value.ToString(),
            review.DinnerId.Value.ToString());
}
