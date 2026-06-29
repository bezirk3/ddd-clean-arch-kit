using Forkfully.Domain.Bill.ValueObjects;
using Forkfully.Domain.Common.Models;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.ValueObjects;

namespace Forkfully.Domain.Dinner.Entities;

// A guest's reservation at a dinner. Local entity of Dinner (bound by the dinner's
// capacity invariant), so it is NOT its own aggregate. BillId is set once billed.
public sealed class Reservation : Entity<ReservationId>
{
    public int GuestCount { get; private set; }
    public GuestId GuestId { get; private set; }
    public BillId BillId { get; private set; }

    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private Reservation(ReservationId reservationId, int guestCount, GuestId guestId, BillId billId)
        : base(reservationId)
    {
        GuestCount = guestCount;
        GuestId = guestId;
        BillId = billId;
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

#pragma warning disable CS8618
    private Reservation()
    {
    }
#pragma warning restore CS8618

    public static Reservation Create(int guestCount, GuestId guestId, BillId billId)
    {
        return new Reservation(
            ReservationId.CreateUnique(),
            guestCount,
            guestId,
            billId);
    }
}
