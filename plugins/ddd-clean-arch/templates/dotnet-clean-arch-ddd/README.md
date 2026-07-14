# Forkfully

A DDD + Clean Architecture .NET solution scaffolded from the `cleanarch-ddd` template.
(The name `Forkfully` is replaced with your `-n` value at scaffold time.)

This template ships the **skeleton plus one working vertical slice — Authentication**
(register / login with JWT issuance, PBKDF2 password hashing, and the `User` aggregate). It does
**not** ship a sample business domain: you add your own aggregates. For a **complete worked
implementation** of this exact architecture — seven aggregates, cross-aggregate domain events,
railway-oriented composition, and 52 ADRs documenting every decision — see the reference solution:

> **Forkfully reference solution** — `d:\repo\local\.poc\ddd-template`
> _(publish it and drop the public URL here)._ Its `docs/adr/` and `docs/design.md` are the
> canonical example of how to grow this skeleton.

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

The only feature present is `Authentication` (in `Application/Authentication`, `Contracts/Authentication`,
`Api/Endpoints/AuthenticationEndpoints.cs`) and its `User` aggregate. Everything else is the
reusable plumbing.

## First run

```bash
# 1. JWT signing secret (dev — HS256 needs >= 32 bytes)
dotnet user-secrets set "JwtSettings:Secret" "<at least 32 bytes>" --project src/Forkfully.Api

# 2. LocalDB instance + schema. This template ships NO migration — create the initial one first.
sqllocaldb create ForkfullyDb && sqllocaldb start ForkfullyDb
dotnet ef migrations add Initial -p src/Forkfully.Infrastructure -s src/Forkfully.Api
dotnet ef database update  -p src/Forkfully.Infrastructure -s src/Forkfully.Api

# 3. build / test / run
dotnet build Forkfully.slnx
dotnet test
dotnet run --project src/Forkfully.Api --launch-profile http
#   docs (Development): http://localhost:5216/scalar/v1  ·  /openapi/v1.json
```

## Next steps

Add your domain on top of the skeleton:

- `aggregate-modeling` — design your aggregates from the problem space.
- `aggregate-slice-generator` — generate each aggregate end-to-end (domain → EF → CQRS → endpoints →
  tests) in the same style the `Authentication` slice demonstrates.
- `adr-keeper` / `design-doc-keeper` — start your decision + design docs.

Refer to the **Forkfully reference solution** (above) whenever you need to see a fully-built example
of any pattern.

> Note: scaffolds currently share the Api's `UserSecretsId`. For isolated dev secrets, drop a fresh
> GUID into `src/Forkfully.Api/Forkfully.Api.csproj`.
