---
name: aggregate-slice-generator
version: 0.1.0
description: |
  Generate a complete vertical slice for a new aggregate in a Clean Architecture + DDD .NET solution:
  domain aggregate (strongly-typed id, ErrorOr factory, invariants, domain event) → EF Core config +
  DbSet → CQRS command/query + handler + validator → contracts DTOs → hand-written mappings →
  minimal-API endpoints (201 + Location) → unit tests. Follows the reference conventions exactly. Use
  when asked to "add an aggregate", "scaffold a feature end-to-end", "generate a vertical slice", or
  "wire up <X>". Proactively suggest after aggregate-modeling identifies a new root.
allowed-tools:
  - Read
  - Write
  - Edit
  - Glob
  - Grep
  - Bash
  - AskUserQuestion
triggers:
  - add an aggregate
  - scaffold a feature end to end
  - generate a vertical slice
  - wire up an aggregate
---

# aggregate-slice-generator

Generate every layer for one aggregate, in the exact style of the reference solution, so the new
code obeys the structural ADRs (and the architecture tests stay green). This is the
"wire-every-aggregate-end-to-end" pattern made repeatable.

Run it **inside a solution scaffolded by [`clean-arch-template`](../clean-arch-template/SKILL.md)**
(or any solution with the same shape). Detect the solution root namespace (the `<Name>` in
`<Name>.Domain` etc.) before generating.

## Inputs

Ask the user (via `AskUserQuestion` if not supplied):

- **Aggregate name** (PascalCase singular, e.g. `Product`).
- **Fields** — name, type, and whether each is a value object / a reference to another aggregate
  (held as a typed id) / a primitive.
- **Invariants** — guard conditions the factory must enforce (e.g. "price > 0", "end > start").
- **Cross-aggregate effects** — does creating it need to update another aggregate? (→ a domain-event
  handler, like `DinnerCreated → Menu.AddDinnerId`.)

## What it generates (the slice manifest)

Replace `<Name>` (root namespace), `<X>` (aggregate, singular), `<Xs>` (plural/route).

| Layer | Files |
| ----- | ----- |
| **Domain** | `<X>/ValueObjects/<X>Id.cs` (`: AggregateRootId<Guid>`, `CreateUnique()` → `Guid.CreateVersion7()`); `<X>/<X>.cs` (`: AggregateRoot<<X>Id, Guid>`, private ctor + setters, static `Create(...) : ErrorOr<<X>>` with guards, raises `<X>Created`); `<X>/Events/<X>Created.cs` (`record … : IDomainEvent`); extend `Common/Errors/Errors.<X>.cs` for each invariant (`Error.Validation(...)`) |
| **Application** | `<Xs>/Commands/Create<X>/` → `Create<X>Command` (`: IRequest<ErrorOr<<X>>>`) + `…Handler` (factory → `_context.<Xs>.Add` → `await SaveChangesAsync`) + `…Validator` (`AbstractValidator`); `<Xs>/Queries/Get<X>/` → `Get<X>Query` + `…Handler` (`SingleOrDefaultAsync`, `Errors.<X>.NotFound`); `Common/Interfaces/IApplicationDbContext` gains `DbSet<<X>> <Xs> { get; }`; register handlers (+ any event handler) in the DI `Configuration/` files |
| **Infrastructure** | `Persistence/Configurations/<X>Configurations.cs` (`IEntityTypeConfiguration<<X>>` — `ToTable`, typed-id `HasConversion` + `ValueGeneratedNever`, owned value objects, backing fields, private setters); `<Name>DbContext` gains `public DbSet<<X>> <Xs> => Set<<X>>();`; then `dotnet ef migrations add Add<X>` |
| **Contracts** | `<Xs>/Create<X>Request.cs`, `<Xs>/<X>Response.cs` (records) |
| **Api** | `Common/Mapping/<X>Mappings.cs` (`ToCommand`/`ToResponse` extension methods, unwrap typed ids via `.Value`, flatten value objects); `Endpoints/<Xs>Endpoints.cs` (`MapGroup("/<xs>")`, `MapPost` → `TypedResults.Created($"/<xs>/{x.Id.Value}", x.ToResponse())`, `MapGet`); register in the `Map<Name>Endpoints` aggregator |
| **Tests** | `tests/unit/<Name>.Domain.UnitTests/<Xs>/<X>Tests.cs` (factory invariants + event raise); `tests/unit/<Name>.Application.UnitTests/<Xs>/Commands/Create<X>/Create<X>CommandValidatorTests.cs` |

Concrete skeletons for every file are in
[`reference/slice-templates.md`](reference/slice-templates.md) — read it before generating and copy
the shapes verbatim, substituting the placeholders.

## Conventions to honor (non-negotiable — the arch tests enforce them)

- **Domain references nothing** but `ErrorOr`; **Application references only Domain** (+ the EF Core
  package for the context seam); **Contracts is standalone**. Never `using` an outer layer inward.
- **Reference other aggregates by typed id** (a value object), never by object.
- **Factories return `ErrorOr<<X>>`** — guard, then construct; never throw for expected failure.
- **Mapping is hand-written static extension methods** — no Mapster/AutoMapper.
- **Dispatch is the in-house `IRequestHandler<,>`** injected per endpoint — no MediatR.
- **Endpoints are minimal APIs** under `Api/Endpoints` — no `*Controller`.
- **Creation returns `201 Created` + `Location`**; reads return `200`; not-found returns `404` via
  the errors catalog.

## Procedure

1. Detect `<Name>` (root namespace) and confirm the solution has the reference shape.
2. Collect inputs (name, fields, invariants, cross-aggregate effects).
3. Generate the files in the manifest order (Domain → Application → Infrastructure → Contracts →
   Api → Tests), following `reference/slice-templates.md`.
4. Add the DI registrations and the `IApplicationDbContext`/`DbContext` `DbSet`, the endpoint
   aggregator line.
5. `dotnet ef migrations add Add<X>` (Infrastructure project, Api startup project).
6. `dotnet build` → 0 errors; `dotnet test` → unit + **architecture** tests green (the arch tests
   confirm the generated code obeys the layering ADRs).
7. Offer to record an ADR (if the aggregate introduced a notable invariant/decision) via
   `adr-keeper`, and to sync the design doc via `design-doc-keeper` — or run the whole thing under
   [`sdlc-loop`](../sdlc-loop/SKILL.md).
