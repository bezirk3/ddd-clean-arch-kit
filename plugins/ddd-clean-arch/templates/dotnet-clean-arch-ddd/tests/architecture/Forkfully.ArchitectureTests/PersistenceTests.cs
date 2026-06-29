using Forkfully.Application.Common.Interfaces;
using Forkfully.ArchitectureTests.Common;
using NetArchTest.Rules;

namespace Forkfully.ArchitectureTests;

// Persistence boundary. The Domain stays persistence-ignorant (ADR-0029), but the
// Application now talks to EF Core through IApplicationDbContext — the DbContext is the
// unit of work — instead of per-aggregate repositories (ADR-0044, which deliberately
// gave up the Application-side persistence ignorance and the repository pattern of
// ADR-0010/0029). Note the dependency *direction* still holds: the Application doesn't
// reference the Infrastructure project (see LayerDependencyTests).
public class PersistenceTests
{
    private const string EntityFrameworkCore = "Microsoft.EntityFrameworkCore";

    [Fact]
    public void Domain_should_not_depend_on_EntityFrameworkCore()
    {
        var result = Types.InAssembly(Layers.DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(EntityFrameworkCore)
            .GetResult();

        result.ShouldBeSuccessful("ADR-0029: the Domain is persistence-ignorant — no EF Core.");
    }

    [Fact]
    public void The_DbContext_should_implement_IApplicationDbContext()
    {
        var result = Types.InAssembly(Layers.InfrastructureAssembly)
            .That()
            .HaveNameEndingWith("DbContext")
            .Should()
            .ImplementInterface(typeof(IApplicationDbContext))
            .GetResult();

        result.ShouldBeSuccessful(
            "ADR-0044: the persistence seam is IApplicationDbContext, implemented by the DbContext in Infrastructure.");
    }
}
