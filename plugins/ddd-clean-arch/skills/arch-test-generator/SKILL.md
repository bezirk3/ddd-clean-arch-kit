---
name: arch-test-generator
version: 0.1.0
description: |
  Generate or refresh a NetArchTest architecture-test project that turns the structural decisions
  (the layering/dependency rule, persistence ignorance, removed libraries, minimal-API and
  domain-event conventions) into a build-time gate. Each test names the offending type and the ADR
  on failure. Use when asked to "add architecture tests", "enforce the layering", "guard the
  dependency rule", or after a structural ADR is recorded. Proactively suggest when a structural
  decision lands without an automated guard.
allowed-tools:
  - Read
  - Write
  - Edit
  - Glob
  - Grep
  - Bash
triggers:
  - add architecture tests
  - enforce the layering
  - guard the dependency rule
  - netarchtest
---

# arch-test-generator

Encode the **structural** ADRs as executable tests with **NetArchTest** (it reads the compiled
assemblies via Mono.Cecil — no running app). Each test asserts a rule and, on failure, names the
offending types **and the ADR**, so a violation is self-explanatory. This is the gate that lets
`sdlc-loop` prove a change still obeys the architecture.

## Project setup (if absent)

Create `tests/architecture/<Name>.ArchitectureTests` — an xUnit project referencing
`NetArchTest.Rules` and **all five** first-party projects. Because the Api is a web project, add
`<FrameworkReference Include="Microsoft.AspNetCore.App" />` so its assembly loads for inspection.

Two helpers under `Common/`:

- **`Layers.cs`** — namespace constants + an anchor `Assembly` per layer (a known type from each),
  plus an `All` array of every first-party assembly for the "no dependency on X" sweeps.
- **`TestResultExtensions.cs`** — `ShouldBeSuccessful(this TestResult, string because)` that asserts
  `result.IsSuccessful` and, on failure, appends `result.FailingTypeNames` so the message names the
  offenders.

```csharp
// Common/Layers.cs — the spine
internal static class Layers
{
    public const string Domain = "<Name>.Domain";
    public const string Application = "<Name>.Application";
    public const string Infrastructure = "<Name>.Infrastructure";
    public const string Api = "<Name>.Api";
    public const string Contracts = "<Name>.Contracts";

    public static readonly Assembly DomainAssembly = typeof(<Name>.Domain.Common.Models.IDomainEvent).Assembly;
    public static readonly Assembly ApplicationAssembly = typeof(<Name>.Application.DependencyInjection).Assembly;
    public static readonly Assembly InfrastructureAssembly = typeof(<Name>.Infrastructure.DependencyInjection).Assembly;
    public static readonly Assembly ApiAssembly = typeof(<Name>.Api.DependencyInjection).Assembly;
    public static readonly Assembly ContractsAssembly = typeof(<Name>.Contracts./*any type*/).Assembly;

    public static readonly Assembly[] All =
        [DomainAssembly, ApplicationAssembly, InfrastructureAssembly, ApiAssembly, ContractsAssembly];
}
```

```csharp
// Common/TestResultExtensions.cs
internal static class TestResultExtensions
{
    public static void ShouldBeSuccessful(this TestResult result, string because)
    {
        var offenders = result.FailingTypeNames is { } names && names.Any()
            ? string.Join(", ", names) : "none";
        Assert.True(result.IsSuccessful, $"{because} (offending types: {offenders})");
    }
}
```

## The rule set (one test file per concern)

Generate the tests that match the decisions actually present in the repo. The reference set (11
tests) — adapt to which ADRs the project has recorded:

| File | Rule | ADR |
| ---- | ---- | --- |
| `LayerDependencyTests` | Domain → nothing; Application → only Domain; Contracts → nothing; Infrastructure ⊀ Api | 0001/0002/0003 |
| `DependencyConstraintTests` | no project references removed libs (e.g. MediatR, Mapster) — sweep `Layers.All` | 0036/0039 |
| `PersistenceTests` | Domain has no EF Core dependency; the `DbContext` implements `IApplicationDbContext` | 0029/0044 |
| `PresentationTests` | no `*Controller`; endpoint classes live under `Api/Endpoints` and end in `Endpoints` | 0038 |
| `DomainModelTests` | domain events implement the `IDomainEvent` marker | 0031 |

### The canonical test shape

```csharp
[Fact]
public void Domain_should_not_depend_on_any_other_layer()
{
    var result = Types.InAssembly(Layers.DomainAssembly)
        .ShouldNot()
        .HaveDependencyOnAny(Layers.Application, Layers.Infrastructure, Layers.Api, Layers.Contracts)
        .GetResult();

    result.ShouldBeSuccessful("ADR-0001: the Domain is the core and depends on nothing outward.");
}
```

Common predicates: `.HaveDependencyOnAny(...)` / `.NotHaveDependencyOnAny(...)`,
`.ResideInNamespace(...)`, `.HaveNameEndingWith(...)`, `.ImplementInterface(typeof(...))`,
`.BeSealed()`. Sweep `Layers.All` in a `foreach` for "no project may reference X" rules.

## Procedure

1. Detect `<Name>` (root namespace) and which structural ADRs exist in `docs/adr/`.
2. If the arch-test project is missing, scaffold it (csproj + the two `Common/` helpers).
3. For each structural ADR, add/refresh the corresponding test file with a `because` message that
   cites the ADR number.
4. `dotnet test` the architecture project — expect all green (a red test means real code violates a
   recorded decision; fix the code, not the test).
5. Report the rule count and any ADR that lacks a guard.
