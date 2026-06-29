using System.Diagnostics;
using Forkfully.Api.Common.Http;
using ErrorOr;

namespace Forkfully.Api.Configuration;

public static class ProblemDetails
{
    // Named distinctly from the framework's IServiceCollection.AddProblemDetails so the call
    // in AddPresentation can't silently bind to the built-in (which would drop the customizer).
    public static IServiceCollection AddApiProblemDetails(this IServiceCollection services)
    {
        // RFC-7807 problem details for minimal APIs (replaces the controller-era
        // ProblemDetailsFactory). The customizer adds the same two extensions the old
        // ForkfullyProblemDetailsFactory did: traceId everywhere, and errorCodes
        // whenever ErrorResults stashed the domain errors.
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["traceId"] =
                    Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

                if (context.HttpContext.Items[HttpContextItemKeys.Errors] is List<Error> errors)
                {
                    context.ProblemDetails.Extensions["errorCodes"] =
                        errors.Select(error => error.Code);
                }
            };
        });

        return services;
    }
}