using Forkfully.ArchitectureTests.Common;
using NetArchTest.Rules;

namespace Forkfully.ArchitectureTests;

// The presentation layer is minimal APIs, not controllers. ADR-0038.
public class PresentationTests
{
    [Fact]
    public void Api_should_not_contain_controllers()
    {
        var result = Types.InAssembly(Layers.ApiAssembly)
            .That()
            .AreClasses()
            .ShouldNot()
            .HaveNameEndingWith("Controller")
            .GetResult();

        result.ShouldBeSuccessful("ADR-0038: the Api uses minimal-API endpoints, not controllers.");
    }

    [Fact]
    public void Endpoint_classes_should_live_in_the_Endpoints_namespace()
    {
        var result = Types.InAssembly(Layers.ApiAssembly)
            .That()
            .HaveNameEndingWith("Endpoints")
            .Should()
            .ResideInNamespace("Forkfully.Api.Endpoints")
            .GetResult();

        result.ShouldBeSuccessful("ADR-0038: minimal-API endpoint groups live under Api/Endpoints.");
    }
}
