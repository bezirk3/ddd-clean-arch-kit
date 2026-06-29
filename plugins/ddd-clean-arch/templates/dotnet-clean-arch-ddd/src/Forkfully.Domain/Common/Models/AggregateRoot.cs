namespace Forkfully.Domain.Common.Models;

// An aggregate root is an entity that is the entry point to its aggregate.
// It re-declares Id as the AggregateRootId<TIdType> base type so that — for EF —
// the root's own Id is a value-converted column, distinct from the strongly-typed
// id (TId) that other aggregates own when they reference this root.
public abstract class AggregateRoot<TId, TIdType> : Entity<TId>
    where TId : AggregateRootId<TIdType>
{
    public new AggregateRootId<TIdType> Id { get; protected set; }

    protected AggregateRoot(TId id) : base(id)
    {
        Id = id;
    }

#pragma warning disable CS8618
    // Parameterless constructor for EF Core materialization (reflection).
    protected AggregateRoot()
    {
    }
#pragma warning restore CS8618
}
