using Forkfully.ArchitectureTests.Common;
using Forkfully.Domain.Common.Models;
using NetArchTest.Rules;

namespace Forkfully.ArchitectureTests;

// DDD building-block conventions in the Domain. ADR-0031 (domain events implement the
// IDomainEvent marker so the dispatcher can find their handlers).
public class DomainModelTests
{
    [Fact]
    public void Domain_events_should_implement_IDomainEvent()
    {
        var result = Types.InAssembly(Layers.DomainAssembly)
            .That()
            .ResideInNamespaceEndingWith(".Events")
            .And()
            .AreClasses()
            .Should()
            .ImplementInterface(typeof(IDomainEvent))
            .GetResult();

        result.ShouldBeSuccessful("ADR-0031: every domain event implements the IDomainEvent marker.");
    }
}
