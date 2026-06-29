namespace Forkfully.Contracts.Guests;

public record CreateGuestRequest(
    string UserId,
    string FirstName,
    string LastName,
    string ProfileImage);

public record GuestResponse(
    string Id,
    string UserId,
    string FirstName,
    string LastName,
    string ProfileImage,
    double AverageRating,
    List<string> UpcomingDinnerIds,
    List<string> PastDinnerIds,
    List<string> BillIds,
    List<string> MenuReviewIds);
