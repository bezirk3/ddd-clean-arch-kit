using Forkfully.Domain.Dinner;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Dinners.Commands.ReserveDinner;

public record ReserveDinnerCommand(
    string DinnerId,
    string GuestId,
    int GuestCount) : IRequest<ErrorOr<Dinner>>;
