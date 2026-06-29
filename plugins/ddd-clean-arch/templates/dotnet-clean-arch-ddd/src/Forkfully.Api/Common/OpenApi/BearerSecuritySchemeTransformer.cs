using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Forkfully.Api.Common.OpenApi;

// The built-in OpenAPI generator doesn't infer the JWT bearer scheme from the auth
// setup, so declare it (and a sensible Info block) in a document transformer. This
// gives Scalar an "Authorize" box where you paste the token from /auth/login and
// applies it to requests; the /auth endpoints are AllowAnonymous, so there the token
// is simply optional.
internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    private const string SchemeId = "Bearer";

    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info ??= new OpenApiInfo();
        document.Info.Title = "Forkfully API";
        document.Info.Version = "v1";
        document.Info.Description =
            "Host dinners, reserve spots, review menus. JWT bearer auth — register or " +
            "log in under /auth, then Authorize with the returned token.";

        var bearerScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Paste the JWT returned by POST /auth/login or /auth/register.",
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes[SchemeId] = bearerScheme;

        // Apply the scheme globally; AllowAnonymous endpoints just ignore the token.
        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(SchemeId, document, null)] = new List<string>(),
        });

        return Task.CompletedTask;
    }
}
