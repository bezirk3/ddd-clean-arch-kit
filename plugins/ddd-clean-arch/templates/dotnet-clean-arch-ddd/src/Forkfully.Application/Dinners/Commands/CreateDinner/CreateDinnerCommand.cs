using Forkfully.Domain.Dinner;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Dinners.Commands.CreateDinner;

public record CreateDinnerCommand(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    int MaxGuestCount,
    decimal Price,
    string Currency,
    string HostId,
    string MenuId,
    string LocationName,
    string LocationAddress,
    double Latitude,
    double Longitude) : IRequest<ErrorOr<Dinner>>;
