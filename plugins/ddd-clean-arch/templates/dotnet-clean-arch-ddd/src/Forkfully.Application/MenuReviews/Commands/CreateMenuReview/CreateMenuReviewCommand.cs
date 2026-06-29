using Forkfully.Domain.MenuReview;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.MenuReviews.Commands.CreateMenuReview;

public record CreateMenuReviewCommand(
    string HostId,
    string MenuId,
    string GuestId,
    string DinnerId,
    int Rating,
    string Comment) : IRequest<ErrorOr<MenuReview>>;
