using Forkfully.Application.Authentication.Commands.Register;
using Forkfully.Application.Authentication.Common;
using Forkfully.Application.Authentication.Queries.Login;
using Forkfully.Application.Common.Messaging;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;

namespace Forkfully.Application.Configuration;

public static class RequestHandlers
{
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
    {
        // Each request handler is registered with its decorator stack (validation +
        // logging) by AddRequestHandler. No assembly scan, no pipeline — the wiring
        // is explicit and you can step straight from the endpoint into the handler.
        // Add one line per use case (see the aggregate-slice-generator skill).
        services
            .AddRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>, RegisterCommandHandler>()
            .AddRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>, LoginQueryHandler>();

        return services;
    }
}
