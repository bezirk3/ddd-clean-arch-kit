using Forkfully.Domain.Common.Models;

namespace Forkfully.Domain.Bill.ValueObjects;

public sealed class BillId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private BillId(Guid value)
    {
        Value = value;
    }

    public static BillId CreateUnique()
    {
        return new BillId(Guid.CreateVersion7());
    }

    public static BillId Create(Guid value)
    {
        return new BillId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
