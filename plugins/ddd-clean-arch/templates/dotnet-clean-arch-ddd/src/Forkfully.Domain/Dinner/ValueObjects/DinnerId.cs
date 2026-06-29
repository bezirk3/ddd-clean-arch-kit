using Forkfully.Domain.Common.Models;

namespace Forkfully.Domain.Dinner.ValueObjects;

public sealed class DinnerId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private DinnerId(Guid value)
    {
        Value = value;
    }

    public static DinnerId CreateUnique()
    {
        return new DinnerId(Guid.CreateVersion7());
    }

    public static DinnerId Create(Guid value)
    {
        return new DinnerId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
