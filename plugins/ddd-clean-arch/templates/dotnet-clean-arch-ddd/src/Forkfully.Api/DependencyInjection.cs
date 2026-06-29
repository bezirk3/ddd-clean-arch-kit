using Forkfully.Api.Configuration;

namespace Forkfully.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddOpenApiDocument();
        services.AddApiProblemDetails();
        services.AddAuthorizationPolicies();

        return services;
    }
}