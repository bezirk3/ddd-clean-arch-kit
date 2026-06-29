using ErrorOr;

namespace Forkfully.Api.Common.Http;

// Minimal-API replacement for the old ApiController.Problem(List<Error>) mapping.
// Same branching as before — empty → 500; all-validation → ValidationProblem with a
// per-field errors map; otherwise map the first error's Type to a status — but it
// returns an IResult instead of living on a controller base class. The traceId and
// errorCodes extensions are added centrally by CustomizeProblemDetails (see
// AddPresentation), which reads the errors stashed below.
public static class ErrorResults
{
    public static IResult From(HttpContext httpContext, List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Results.Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            // Group by code (the field) → { field: [messages] }, matching the old
            // ModelStateDictionary-based ValidationProblem.
            var failures = errors
                .GroupBy(error => error.Code)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.Description).ToArray());

            return Results.ValidationProblem(failures);
        }

        // Stash the errors so CustomizeProblemDetails can surface their codes.
        httpContext.Items[HttpContextItemKeys.Errors] = errors;

        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Results.Problem(statusCode: statusCode, title: firstError.Description);
    }

    // For the per-endpoint status override (e.g. Login forcing 401). No errors are
    // stashed, so the response carries no errorCodes — same as the old override.
    public static IResult From(HttpContext httpContext, int statusCode, string title) =>
        Results.Problem(statusCode: statusCode, title: title);
}
