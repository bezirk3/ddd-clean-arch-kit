using Forkfully.Application.Common.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Forkfully.Application.Configuration;

public static class EventHandlers
{
    public static IServiceCollection AddEventHandlers(this IServiceCollection services)
    {
        // Domain-event dispatch (replaces MediatR's IPublisher / INotificationHandler).
        // Register a handler per domain event you react to:
        //   services.AddScoped<IDomainEventHandler<SomethingHappened>, SomethingHandler>();
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

        return services;
    }
}
