using Forkfully.Domain.Common.Models;

namespace Forkfully.Application.Common.Messaging;

// Replaces MediatR's INotificationHandler<T> for domain events. A domain event can
// have zero or more handlers; all registered handlers for an event run when it's
// dispatched (see IDomainEventsDispatcher).
public interface IDomainEventHandler<in TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
}
