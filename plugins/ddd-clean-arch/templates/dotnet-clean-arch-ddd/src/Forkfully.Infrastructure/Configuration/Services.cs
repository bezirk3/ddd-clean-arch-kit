using Forkfully.Application.Common.Interfaces.Services;
using Forkfully.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Forkfully.Infrastructure.Configuration;

public static class Services
{
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}