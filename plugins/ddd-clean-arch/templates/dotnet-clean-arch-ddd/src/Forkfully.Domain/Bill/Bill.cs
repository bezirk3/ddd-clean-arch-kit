using Forkfully.Domain.Bill.ValueObjects;
using Forkfully.Domain.Common.Models;
using Forkfully.Domain.Common.ValueObjects;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.ValueObjects;

namespace Forkfully.Domain.Bill;

// What a guest paid for a reservation at a dinner.
public sealed class Bill : AggregateRoot<BillId, Guid>
{
    public DinnerId DinnerId { get; private set; }
    public GuestId GuestId { get; private set; }
    public Price Amount { get; private set; }

    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private Bill(BillId billId, DinnerId dinnerId, GuestId guestId, Price amount)
        : base(billId)
    {
        DinnerId = dinnerId;
        GuestId = guestId;
        Amount = amount;
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

#pragma warning disable CS8618
    private Bill()
    {
    }
#pragma warning restore CS8618

    public static Bill Create(DinnerId dinnerId, GuestId guestId, Price amount)
    {
        return new Bill(
            BillId.CreateUnique(),
            dinnerId,
            guestId,
            amount);
    }
}
