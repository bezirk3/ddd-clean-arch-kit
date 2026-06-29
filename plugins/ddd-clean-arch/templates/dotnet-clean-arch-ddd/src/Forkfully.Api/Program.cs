using Forkfully.Api;
using Forkfully.Api.Endpoints;
using Forkfully.Application;
using Forkfully.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.AddServiceDefaults();

var app = builder.Build();

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapForkfullyEndpoints();

// API docs (Development only): the OpenAPI 3.1 document + Scalar UI. Both are
// AllowAnonymous so the secure-by-default fallback policy doesn't lock them out.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();                  // /openapi/v1.json (OpenAPI 3.1)
    app.MapScalarApiReference(options =>                // /scalar/v1
    {
        options.Title = "Forkfully API";
        options.DarkMode = true;
    }).AllowAnonymous();
}

// Re-execution target for UseExceptionHandler — only unexpected exceptions reach it.
// AddProblemDetails() turns the empty Results.Problem() into a safe RFC-7807 500
// (traceId, no stack trace). Anonymous so a faulted request isn't blocked by the
// fallback authorization policy.
app.Map("/error", () => Results.Problem())
    .AllowAnonymous()
    .ExcludeFromDescription();

await app.RunAsync();
