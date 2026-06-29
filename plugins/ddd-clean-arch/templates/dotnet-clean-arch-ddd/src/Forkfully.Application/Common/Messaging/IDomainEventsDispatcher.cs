using Forkfully.Domain.Common.Models;

namespace Forkfully.Application.Common.Messaging;

// Replaces MediatR's IPublisher for the domain-event flow. The SaveChanges
// interceptor collects the events raised by tracked aggregates and hands them here;
// the dispatcher resolves and invokes each event's handlers.
public interface IDomainEventsDispatcher
{
    Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default);
}
