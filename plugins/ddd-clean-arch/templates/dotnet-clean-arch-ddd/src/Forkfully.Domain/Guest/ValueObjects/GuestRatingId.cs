using Forkfully.Domain.Common.Models;

namespace Forkfully.Domain.Guest.ValueObjects;

// Entity id (not an aggregate root) — a plain value object.
public sealed class GuestRatingId : ValueObject
{
    public Guid Value { get; }

    private GuestRatingId(Guid value)
    {
        Value = value;
    }

    public static GuestRatingId CreateUnique()
    {
        return new GuestRatingId(Guid.CreateVersion7());
    }

    public static GuestRatingId Create(Guid value)
    {
        return new GuestRatingId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
