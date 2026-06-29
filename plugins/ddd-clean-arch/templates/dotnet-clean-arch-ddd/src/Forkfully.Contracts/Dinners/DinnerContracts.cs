namespace Forkfully.Contracts.Dinners;

public record CreateDinnerRequest(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    int MaxGuestCount,
    PriceDto Price,
    string HostId,
    string MenuId,
    LocationDto Location);

public record ReserveDinnerRequest(
    string GuestId,
    int GuestCount);

public record PriceDto(decimal Amount, string Currency);

public record LocationDto(string Name, string Address, double Latitude, double Longitude);

public record DinnerResponse(
    string Id,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Status,
    int MaxGuestCount,
    PriceDto Price,
    string HostId,
    string MenuId,
    LocationDto Location,
    List<ReservationResponse> Reservations);

public record ReservationResponse(
    string Id,
    int GuestCount,
    string GuestId,
    string BillId);
