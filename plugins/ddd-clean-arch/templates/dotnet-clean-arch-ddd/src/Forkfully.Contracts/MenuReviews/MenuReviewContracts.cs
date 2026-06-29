namespace Forkfully.Contracts.MenuReviews;

public record CreateMenuReviewRequest(
    string HostId,
    string MenuId,
    string GuestId,
    string DinnerId,
    int Rating,
    string Comment);

public record MenuReviewResponse(
    string Id,
    int Rating,
    string Comment,
    string HostId,
    string MenuId,
    string GuestId,
    string DinnerId);
