using Forkfully.Application.Guests.Commands.CreateGuest;
using Forkfully.Contracts.Guests;
using DomainGuest = Forkfully.Domain.Guest.Guest;

namespace Forkfully.Api.Common.Mapping;

public static class GuestMappings
{
    public static CreateGuestCommand ToCommand(this CreateGuestRequest request) =>
        new(request.UserId, request.FirstName, request.LastName, request.ProfileImage);

    public static GuestResponse ToResponse(this DomainGuest guest) =>
        new(
            guest.Id.Value.ToString(),
            guest.UserId.Value.ToString(),
            guest.FirstName,
            guest.LastName,
            guest.ProfileImage,
            guest.AverageRating.Value,
            guest.UpcomingDinnerIds.Select(id => id.Value.ToString()).ToList(),
            guest.PastDinnerIds.Select(id => id.Value.ToString()).ToList(),
            guest.BillIds.Select(id => id.Value.ToString()).ToList(),
            guest.MenuReviewIds.Select(id => id.Value.ToString()).ToList());
}
