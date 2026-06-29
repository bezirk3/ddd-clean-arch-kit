using Forkfully.Api.Common.OpenApi;

namespace Forkfully.Api.Configuration;

public static class OpenApi
{
    // Named distinctly from the framework's IServiceCollection.AddOpenApi so the call in
    // AddPresentation can't silently bind to the built-in (which would drop the transformer).
    public static IServiceCollection AddOpenApiDocument(this IServiceCollection services)
    {
        // OpenAPI 3.1 document via the built-in (AOT-ready) generator; the transformer
        // adds the JWT bearer scheme + Info. Served by MapOpenApi and rendered by
        // Scalar (see Program.cs).
        services.AddOpenApi(options =>
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());
            
        return services;
    }
}