using Forkfully.Domain.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Forkfully.Application.Common.Messaging;

// Resolves and runs the handlers for each domain event. Events are collected as the
// base IDomainEvent, so we lean on a `dynamic` dispatch to recover the concrete
// event type T and ask the container for its IDomainEventHandler<T> set — the one
// spot where a touch of late binding earns its keep (heterogeneous events, n
// handlers each). No reflection-by-MethodInfo, no MediatR.
public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventsDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            await DispatchAsync((dynamic)domainEvent, cancellationToken);
        }
    }

    private async Task DispatchAsync<TDomainEvent>(
        TDomainEvent domainEvent,
        CancellationToken cancellationToken)
        where TDomainEvent : IDomainEvent
    {
        var handlers = _serviceProvider.GetServices<IDomainEventHandler<TDomainEvent>>();

        foreach (var handler in handlers)
        {
            await handler.Handle(domainEvent, cancellationToken);
        }
    }
}
