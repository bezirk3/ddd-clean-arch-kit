using Forkfully.Domain.Common.Models;

namespace Forkfully.Domain.Dinner.ValueObjects;

// Entity id (not an aggregate root) — a plain value object.
public sealed class ReservationId : ValueObject
{
    public Guid Value { get; }

    private ReservationId(Guid value)
    {
        Value = value;
    }

    public static ReservationId CreateUnique()
    {
        return new ReservationId(Guid.CreateVersion7());
    }

    public static ReservationId Create(Guid value)
    {
        return new ReservationId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
