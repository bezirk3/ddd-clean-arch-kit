# Forkfully

A DDD + Clean Architecture .NET solution scaffolded from the `cleanarch-ddd` template.
(The name `Forkfully` is replaced with your `-n` value at scaffold time.)

## Layout

```
Forkfully.slnx
src/
  Forkfully.Domain          aggregates, value objects, domain events, errors catalog (depends on nothing)
  Forkfully.Application     CQRS use cases, in-house IRequest/IRequestHandler + decorators, IApplicationDbContext
  Forkfully.Infrastructure  EF Core DbContext, JWT, clock, password hashing
  Forkfully.Contracts       public API DTOs
  Forkfully.Api             minimal-API endpoints, ProblemDetails, OpenAPI 3.1 + Scalar, composition root
  Forkfully.ServiceDefaults / Forkfully.AppHost   .NET Aspire observability + local orchestration
tests/
  unit/          one test project per source project
  architecture/  NetArchTest rules that encode the layering decisions
```

## First run

```bash
# 1. JWT signing secret (dev — HS256 needs >= 32 bytes)
dotnet user-secrets set "JwtSettings:Secret" "<at least 32 bytes>" --project src/Forkfully.Api

# 2. LocalDB instance + schema (connection string targets (localdb)\ForkfullyDb)
sqllocaldb create ForkfullyDb && sqllocaldb start ForkfullyDb
dotnet ef database update -p src/Forkfully.Infrastructure -s src/Forkfully.Api

# 3. build / test / run
dotnet build Forkfully.slnx
dotnet test
dotnet run --project src/Forkfully.Api --launch-profile http
#   docs (Development): http://localhost:5216/scalar/v1  ·  /openapi/v1.json
```

## Next steps

The solution ships a **dinner-hosting reference domain** as a worked example. Replace the sample
aggregates with your own — the `aggregate-slice-generator` skill adds new aggregates end-to-end in
this same style, and `adr-keeper` / `design-doc-keeper` start your decision + design docs.

> Note: scaffolds currently share the Api's `UserSecretsId`. For isolated dev secrets, drop a fresh
> GUID into `src/Forkfully.Api/Forkfully.Api.csproj`.
