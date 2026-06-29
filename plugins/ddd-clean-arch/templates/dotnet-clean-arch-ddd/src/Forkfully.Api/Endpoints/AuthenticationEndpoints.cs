using Forkfully.Api.Common.Http;
using Forkfully.Api.Common.Mapping;
using Forkfully.Application.Authentication.Commands.Register;
using Forkfully.Application.Authentication.Common;
using Forkfully.Application.Authentication.Queries.Login;
using Forkfully.Application.Common.Messaging;
using Forkfully.Contracts.Authentication;
using Forkfully.Domain.Common.Errors;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Forkfully.Api.Endpoints;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder routes)
    {
        // The only anonymous group — everything else is secured by the fallback policy.
        var group = routes.MapGroup("/auth").AllowAnonymous();

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);

        return routes;
    }

    public static async Task<IResult> Register(
        RegisterRequest request,
        [FromServices] IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var authResult = await handler.Handle(request.ToCommand(), cancellationToken);

        return authResult.Match<IResult>(
            result => TypedResults.Ok(result.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }

    public static async Task<IResult> Login(
        LoginRequest request,
        [FromServices] IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var authResult = await handler.Handle(request.ToQuery(), cancellationToken);

        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return ErrorResults.From(
                httpContext,
                StatusCodes.Status401Unauthorized,
                authResult.FirstError.Description);
        }

        return authResult.Match<IResult>(
            result => TypedResults.Ok(result.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }
}
