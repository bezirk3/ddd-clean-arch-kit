using Forkfully.Domain.Bill.ValueObjects;
using Forkfully.Domain.Common.Errors;
using Forkfully.Domain.Common.Models;
using Forkfully.Domain.Common.ValueObjects;
using Forkfully.Domain.Dinner.Entities;
using Forkfully.Domain.Dinner.Enums;
using Forkfully.Domain.Dinner.Events;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu.ValueObjects;
using ErrorOr;

namespace Forkfully.Domain.Dinner;

// A single hosted dinner, run against a Menu. Owns its reservations as local
// entities so it can enforce the capacity invariant atomically.
public sealed class Dinner : AggregateRoot<DinnerId, Guid>
{
    private readonly List<Reservation> _reservations = new();

    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public DateTime? StartedDateTime { get; private set; }
    public DateTime? EndedDateTime { get; private set; }
    public DinnerStatus Status { get; private set; }
    public int MaxGuestCount { get; private set; }
    public Price Price { get; private set; }
    public HostId HostId { get; private set; }
    public MenuId MenuId { get; private set; }
    public Location Location { get; private set; }

    public IReadOnlyList<Reservation> Reservations => _reservations.AsReadOnly();

    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private Dinner(
        DinnerId dinnerId,
        string name,
        string description,
        DateTime startDateTime,
        DateTime endDateTime,
        int maxGuestCount,
        Price price,
        HostId hostId,
        MenuId menuId,
        Location location)
        : base(dinnerId)
    {
        Name = name;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        MaxGuestCount = maxGuestCount;
        Price = price;
        HostId = hostId;
        MenuId = menuId;
        Location = location;
        Status = DinnerStatus.Upcoming;
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

#pragma warning disable CS8618
    private Dinner()
    {
    }
#pragma warning restore CS8618

    public static ErrorOr<Dinner> Create(
        string name,
        string description,
        DateTime startDateTime,
        DateTime endDateTime,
        int maxGuestCount,
        Price price,
        HostId hostId,
        MenuId menuId,
        Location location)
    {
        // Domain invariants — the factory cannot mint an invalid dinner, regardless of
        // whether an application-layer validator ran first (defense-in-depth).
        if (endDateTime <= startDateTime)
        {
            return Errors.Dinner.EndBeforeStart;
        }

        if (maxGuestCount <= 0)
        {
            return Errors.Dinner.InvalidGuestCount;
        }

        var dinner = new Dinner(
            DinnerId.CreateUnique(),
            name,
            description,
            startDateTime,
            endDateTime,
            maxGuestCount,
            price,
            hostId,
            menuId,
            location);

        dinner.AddDomainEvent(new DinnerCreated(dinner));

        return dinner;
    }

    // Invariant: Σ reservation guest counts ≤ MaxGuestCount — enforced atomically here.
    // The bill is created by the caller first; we attach it to the new reservation.
    public ErrorOr<Success> ReserveSpot(GuestId guestId, int guestCount, BillId billId)
    {
        var reservedCount = _reservations.Sum(reservation => reservation.GuestCount);
        if (reservedCount + guestCount > MaxGuestCount)
        {
            return Errors.Dinner.CannotReserveMoreSpotsThanAvailable;
        }

        _reservations.Add(Reservation.Create(guestCount, guestId, billId));
        return Result.Success;
    }

    public void CancelReservation(ReservationId reservationId)
    {
        _reservations.RemoveAll(reservation => reservation.Id == reservationId);
    }

    public void StartDinner()
    {
        Status = DinnerStatus.InProgress;
        StartedDateTime = DateTime.UtcNow;
    }

    public void EndDinner()
    {
        Status = DinnerStatus.Ended;
        EndedDateTime = DateTime.UtcNow;
    }
}
