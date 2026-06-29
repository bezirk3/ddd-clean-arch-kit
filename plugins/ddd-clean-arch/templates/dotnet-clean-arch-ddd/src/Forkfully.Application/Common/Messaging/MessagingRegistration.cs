using Forkfully.Application.Common.Behaviors;
using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Forkfully.Application.Common.Messaging;

// Replaces MediatR's assembly scan + pipeline registration. Registers one handler
// and wraps it in the decorator stack, by hand, with full types — so you can see
// (and step through) exactly what runs and in what order. This is the DI-based
// decoration the "you don't need MediatR" approach is built on: no magic dispatch,
// no package. Decorate explicitly here, per handler, so what gets wrapped is a
// deliberate choice rather than a blanket convention.
public static class MessagingRegistration
{
    public static IServiceCollection AddRequestHandler<TRequest, TResponse, THandler>(
        this IServiceCollection services)
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
        where THandler : class, IRequestHandler<TRequest, TResponse>
    {
        // The real handler — resolvable on its own so its dependencies inject normally.
        services.AddScoped<THandler>();

        // The public IRequestHandler<TRequest, TResponse> resolves to the handler
        // wrapped innermost-out: handler → validation → logging.
        services.AddScoped<IRequestHandler<TRequest, TResponse>>(serviceProvider =>
        {
            IRequestHandler<TRequest, TResponse> handler =
                serviceProvider.GetRequiredService<THandler>();

            handler = new ValidationDecorator<TRequest, TResponse>(
                handler,
                serviceProvider.GetService<IValidator<TRequest>>());

            handler = new LoggingDecorator<TRequest, TResponse>(
                handler,
                serviceProvider.GetRequiredService<ILogger<LoggingDecorator<TRequest, TResponse>>>());

            return handler;
        });

        return services;
    }
}
