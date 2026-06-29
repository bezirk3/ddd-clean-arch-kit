using Forkfully.Application.Dinners.Commands.CreateDinner;
using Forkfully.Contracts.Dinners;
using DomainDinner = Forkfully.Domain.Dinner.Dinner;

namespace Forkfully.Api.Common.Mapping;

public static class DinnerMappings
{
    // Flatten the nested Price/Location DTOs into the flat command.
    public static CreateDinnerCommand ToCommand(this CreateDinnerRequest request) =>
        new(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.MaxGuestCount,
            request.Price.Amount,
            request.Price.Currency,
            request.HostId,
            request.MenuId,
            request.Location.Name,
            request.Location.Address,
            request.Location.Latitude,
            request.Location.Longitude);

    public static DinnerResponse ToResponse(this DomainDinner dinner) =>
        new(
            dinner.Id.Value.ToString(),
            dinner.Name,
            dinner.Description,
            dinner.StartDateTime,
            dinner.EndDateTime,
            dinner.Status.ToString(),
            dinner.MaxGuestCount,
            new PriceDto(dinner.Price.Amount, dinner.Price.Currency),
            dinner.HostId.Value.ToString(),
            dinner.MenuId.Value.ToString(),
            new LocationDto(
                dinner.Location.Name,
                dinner.Location.Address,
                dinner.Location.Latitude,
                dinner.Location.Longitude),
            dinner.Reservations.Select(reservation => new ReservationResponse(
                reservation.Id.Value.ToString(),
                reservation.GuestCount,
                reservation.GuestId.Value.ToString(),
                reservation.BillId.Value.ToString())).ToList());
}
