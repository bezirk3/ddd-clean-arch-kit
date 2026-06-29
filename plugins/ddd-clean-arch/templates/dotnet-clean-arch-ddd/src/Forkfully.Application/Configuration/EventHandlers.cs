using Forkfully.Application.Common.Messaging;
using Forkfully.Application.Dinners.Events;
using Forkfully.Domain.Dinner.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Forkfully.Application.Configuration;

public static class EventHandlers
{
    public static IServiceCollection AddEventHandlers(this IServiceCollection services)
    {
        // Domain-event dispatch (replaces MediatR's IPublisher / INotificationHandler).
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.AddScoped<IDomainEventHandler<DinnerCreated>, DinnerCreatedEventHandler>();

        return services;
    }
}