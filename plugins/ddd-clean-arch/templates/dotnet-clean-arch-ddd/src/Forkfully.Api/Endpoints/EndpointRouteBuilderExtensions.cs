namespace Forkfully.Api.Endpoints;

// Single entry point the host calls instead of MapControllers. The "downside" of
// minimal APIs — having to remember each MapXEndpoints — is contained here: one
// aggregator, mapped once from Program.cs. (Carter would automate this via assembly
// scanning, but at the cost of a dependency we chose not to take.)
public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapAuthenticationEndpoints();
        routes.MapHostEndpoints();
        routes.MapGuestEndpoints();
        routes.MapMenuEndpoints();
        routes.MapDinnerEndpoints();
        routes.MapBillEndpoints();
        routes.MapMenuReviewEndpoints();

        return routes;
    }
}
