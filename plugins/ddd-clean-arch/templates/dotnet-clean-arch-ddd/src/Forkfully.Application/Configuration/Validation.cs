using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Forkfully.Application.Configuration;

public static class Validation
{
    public static IServiceCollection FluentValidation(this IServiceCollection services)
    {
        // FluentValidation validators — resolved (when present) by ValidationDecorator.
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}