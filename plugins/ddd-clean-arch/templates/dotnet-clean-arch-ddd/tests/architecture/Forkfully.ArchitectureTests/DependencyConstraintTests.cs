using Forkfully.ArchitectureTests.Common;
using NetArchTest.Rules;

namespace Forkfully.ArchitectureTests;

// Libraries we deliberately removed must stay removed.
// ADR-0036 (MediatR replaced by in-house messaging + DI decoration),
// ADR-0039 (Mapster replaced by hand-written mapping methods).
public class DependencyConstraintTests
{
    [Fact]
    public void No_project_should_depend_on_MediatR()
    {
        foreach (var assembly in Layers.All)
        {
            var result = Types.InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny("MediatR")
                .GetResult();

            result.ShouldBeSuccessful(
                $"ADR-0036: MediatR was removed — {assembly.GetName().Name} must not reference it.");
        }
    }

    [Fact]
    public void No_project_should_depend_on_Mapster()
    {
        foreach (var assembly in Layers.All)
        {
            var result = Types.InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny("Mapster", "MapsterMapper")
                .GetResult();

            result.ShouldBeSuccessful(
                $"ADR-0039: Mapster was removed — {assembly.GetName().Name} must not reference it.");
        }
    }
}
