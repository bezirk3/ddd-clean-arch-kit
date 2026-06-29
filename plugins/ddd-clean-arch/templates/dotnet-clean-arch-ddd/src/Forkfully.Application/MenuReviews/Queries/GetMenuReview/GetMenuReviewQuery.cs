using Forkfully.Domain.MenuReview;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.MenuReviews.Queries.GetMenuReview;

public record GetMenuReviewQuery(string MenuReviewId) : IRequest<ErrorOr<MenuReview>>;
