using Forkfully.Domain.Guest;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Guests.Queries.GetGuest;

public record GetGuestQuery(string GuestId) : IRequest<ErrorOr<Guest>>;
