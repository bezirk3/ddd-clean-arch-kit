using Microsoft.Extensions.DependencyInjection;
using Forkfully.Application.Configuration;

namespace Forkfully.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.FluentValidation();
        services.AddRequestHandlers();
        services.AddEventHandlers();

        return services;
    }
}