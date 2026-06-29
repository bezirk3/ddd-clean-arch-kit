using Forkfully.Domain.Common.Models;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;

namespace Forkfully.Domain.Guest.Entities;

// A host's 1–5 rating of a guest, scoped to a dinner. Local entity of Guest.
public sealed class GuestRating : Entity<GuestRatingId>
{
    public HostId HostId { get; private set; }
    public DinnerId DinnerId { get; private set; }
    public int Rating { get; private set; }

    private GuestRating(GuestRatingId guestRatingId, HostId hostId, DinnerId dinnerId, int rating)
        : base(guestRatingId)
    {
        HostId = hostId;
        DinnerId = dinnerId;
        Rating = rating;
    }

#pragma warning disable CS8618
    private GuestRating()
    {
    }
#pragma warning restore CS8618

    public static GuestRating Create(HostId hostId, DinnerId dinnerId, int rating)
    {
        return new GuestRating(
            GuestRatingId.CreateUnique(),
            hostId,
            dinnerId,
            rating);
    }
}
