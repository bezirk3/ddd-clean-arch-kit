---
name: clean-arch-template
version: 0.1.0
description: |
  Scaffold a new .NET solution from the bundled Clean Architecture + DDD dotnet new template — a
  7-project layout (Domain/Application/Infrastructure/Contracts/Api + Aspire ServiceDefaults/AppHost)
  with ErrorOr flow control, in-house CQRS dispatch, EF Core via IApplicationDbContext, OpenAPI +
  Scalar, JWT auth, and architecture tests. Use when asked to "start a new clean architecture
  project", "scaffold a DDD solution", "new dotnet ddd app", or "use the template". Proactively
  suggest at the start of a new .NET service.
allowed-tools:
  - Bash
  - Read
  - Edit
  - Glob
  - AskUserQuestion
triggers:
  - new clean architecture project
  - scaffold a ddd solution
  - new dotnet ddd app
  - use the template
---

# clean-arch-template

Scaffold a fresh solution from the `dotnet new` template that ships with this plugin
(`templates/dotnet-clean-arch-ddd/`). The template is the *Forkfully* reference solution with its
name parameterized via `sourceName` — `dotnet new -n MyApp` renames `Forkfully` → `MyApp` across
every file, folder, namespace, project, and the `.slnx`, using the same token replacement proven
safe in the reference repo (but done declaratively by the templating engine).

## What you get

A 7-project Clean Architecture + DDD solution:

| Project | Role |
| ------- | ---- |
| `<Name>.Domain` | aggregates, value objects, domain events, errors catalog (depends on nothing) |
| `<Name>.Application` | CQRS use cases, in-house `IRequest`/`IRequestHandler` + validation/logging decorators, `IApplicationDbContext` |
| `<Name>.Infrastructure` | EF Core `DbContext`, JWT, clock, password hashing |
| `<Name>.Contracts` | public API DTOs |
| `<Name>.Api` | minimal-API endpoints, ProblemDetails, OpenAPI 3.1 + Scalar, composition root |
| `<Name>.ServiceDefaults` / `<Name>.AppHost` | .NET Aspire observability + local orchestration |
| `tests/` | unit tests + a NetArchTest architecture-test project |

## Scaffold

```bash
# one-time: register the template with the .NET CLI (path to the bundled payload)
dotnet new install <plugin-dir>/templates/dotnet-clean-arch-ddd

# create a new solution
dotnet new cleanarch-ddd -n Acme.Orders -o ./acme-orders
```

`-n` sets the solution/namespace root; `-o` the output directory.

## Post-scaffold runbook

The generated solution needs three things before it runs end-to-end (the template can't ship a dev
secret or a database):

1. **JWT signing secret** (dev — user secrets; HS256 needs ≥ 32 bytes):
   ```bash
   dotnet user-secrets set "JwtSettings:Secret" "<at least 32 bytes>" --project src/Acme.Orders.Api
   ```
2. **LocalDB instance + database** (the connection string targets `(localdb)\<Name>Db`):
   ```bash
   sqllocaldb create Acme.OrdersDb && sqllocaldb start Acme.OrdersDb
   dotnet ef database update -p src/Acme.Orders.Infrastructure -s src/Acme.Orders.Api
   ```
3. **Build, test, run:**
   ```bash
   dotnet build Acme.Orders.slnx
   dotnet test                      # unit + architecture tests
   dotnet run --project src/Acme.Orders.Api --launch-profile http
   #   docs (Dev): http://localhost:5216/scalar/v1  ·  /openapi/v1.json
   ```

## Procedure

1. Ask the user for the solution name (`-n`) and output dir (`-o`) if not given.
2. `dotnet new install` the bundled template payload (idempotent — `--force` to update).
3. `dotnet new cleanarch-ddd -n <Name> -o <dir>`.
4. Walk the post-scaffold runbook; offer to run build + tests to confirm green.
5. Suggest next steps: `aggregate-modeling` to design the domain, then
   [`aggregate-slice-generator`](../aggregate-slice-generator/SKILL.md) to add aggregates, and
   `adr-keeper` + `design-doc-keeper` to start the project's docs.

## Notes / known limitations

- The template ships the dinner-hosting reference domain as a **worked example**. Replace the sample
  aggregates with your own (or strip them) — `aggregate-slice-generator` adds new ones in the same
  style.
- All scaffolds currently share the Api's `UserSecretsId` GUID; for isolated dev secrets, generate a
  fresh GUID into `src/<Name>.Api/<Name>.Api.csproj` after scaffolding. (A future template `symbol`
  will auto-generate it.)
