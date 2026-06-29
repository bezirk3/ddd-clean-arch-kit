namespace Forkfully.Domain.Common.Models;

// The id of an aggregate root. It is a distinct type from the typed id that other
// aggregates use to *reference* this root — which lets EF Core configure the root's
// own Id as a value-converted column while the references are owned entities,
// avoiding the "same type configured both ways" conflict.
public abstract class AggregateRootId<TId> : ValueObject
{
    public abstract TId Value { get; protected set; }
}
