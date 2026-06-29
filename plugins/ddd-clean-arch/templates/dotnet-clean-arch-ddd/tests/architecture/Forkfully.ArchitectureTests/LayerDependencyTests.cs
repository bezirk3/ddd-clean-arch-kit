using Forkfully.ArchitectureTests.Common;
using NetArchTest.Rules;

namespace Forkfully.ArchitectureTests;

// Clean Architecture dependency rule: source dependencies point inward only.
// ADR-0001 (clean architecture), ADR-0002 (Contracts standalone), ADR-0003 (Api is
// the composition root that references Infrastructure — not the reverse).
public class LayerDependencyTests
{
    [Fact]
    public void Domain_should_not_depend_on_any_other_layer()
    {
        var result = Types.InAssembly(Layers.DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(Layers.Application, Layers.Infrastructure, Layers.Api, Layers.Contracts)
            .GetResult();

        result.ShouldBeSuccessful("ADR-0001: the Domain is the core and depends on nothing outward.");
    }

    [Fact]
    public void Application_should_not_depend_on_outer_layers()
    {
        var result = Types.InAssembly(Layers.ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(Layers.Infrastructure, Layers.Api, Layers.Contracts)
            .GetResult();

        result.ShouldBeSuccessful(
            "ADR-0001/0002: Application depends only on Domain — not Infrastructure, Api, or Contracts.");
    }

    [Fact]
    public void Contracts_should_not_depend_on_any_other_layer()
    {
        var result = Types.InAssembly(Layers.ContractsAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(Layers.Domain, Layers.Application, Layers.Infrastructure, Layers.Api)
            .GetResult();

        result.ShouldBeSuccessful(
            "ADR-0002: Contracts are standalone public DTOs — they reference no other layer.");
    }

    [Fact]
    public void Infrastructure_should_not_depend_on_the_Api()
    {
        var result = Types.InAssembly(Layers.InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(Layers.Api)
            .GetResult();

        result.ShouldBeSuccessful(
            "ADR-0001/0003: Infrastructure is an adapter; the Api (composition root) depends on it, not the reverse.");
    }
}
