namespace Forkfully.Domain.Common.Models;

// A plain marker — a domain event can have zero or more handlers. The Domain layer
// owns this interface and depends on nothing (no MediatR); the Application layer
// defines IDomainEventHandler<T> over it and dispatches them.
public interface IDomainEvent;
