using NetArchTest.Rules;

namespace Forkfully.ArchitectureTests.Common;

internal static class TestResultExtensions
{
    // Asserts the rule held, and on failure names the offending types and the ADR.
    public static void ShouldBeSuccessful(this TestResult result, string because)
    {
        var offenders = result.FailingTypeNames is { } names && names.Any()
            ? string.Join(", ", names)
            : "none";

        Assert.True(result.IsSuccessful, $"{because} (offending types: {offenders})");
    }
}
