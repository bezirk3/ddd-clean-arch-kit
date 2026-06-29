using Microsoft.AspNetCore.Authorization;

namespace Forkfully.Api.Configuration;

public static class Authorization
{
    // Named distinctly from the framework's IServiceCollection.AddAuthorization so the call in
    // AddPresentation can't silently bind to the built-in — which would skip the FallbackPolicy
    // and leave every endpoint reachable without authentication (ADR-0022).
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        // Secure by default — the minimal-API equivalent of `[Authorize]` on the old
        // ApiController base. Every endpoint requires an authenticated user unless it
        // opts out with AllowAnonymous (only the /auth group and /error do).
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}