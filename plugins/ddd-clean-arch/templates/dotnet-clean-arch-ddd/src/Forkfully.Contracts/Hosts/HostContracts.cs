namespace Forkfully.Contracts.Hosts;

public record CreateHostRequest(
    string UserId,
    string FirstName,
    string LastName,
    string ProfileImage);

public record HostResponse(
    string Id,
    string UserId,
    string FirstName,
    string LastName,
    string ProfileImage,
    double AverageRating,
    List<string> MenuIds,
    List<string> DinnerIds);
