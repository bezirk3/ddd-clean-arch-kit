using Forkfully.Application.Common.Interfaces;
using Forkfully.Infrastructure.Persistence;
using Forkfully.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forkfully.Infrastructure.Configuration;

public static class Persistence
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddDbContext<ForkfullyDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Forkfully")));

        // The Application depends on the context through its own interface; resolve it
        // to the same scoped ForkfullyDbContext instance.
        services.AddScoped<IApplicationDbContext>(
            serviceProvider => serviceProvider.GetRequiredService<ForkfullyDbContext>());

        services.AddScoped<PublishDomainEventsInterceptor>();

        return services;
    }
}