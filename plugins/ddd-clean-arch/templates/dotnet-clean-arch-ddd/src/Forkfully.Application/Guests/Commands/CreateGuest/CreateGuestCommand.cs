using Forkfully.Domain.Guest;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Guests.Commands.CreateGuest;

public record CreateGuestCommand(
    string UserId,
    string FirstName,
    string LastName,
    string ProfileImage) : IRequest<ErrorOr<Guest>>;
