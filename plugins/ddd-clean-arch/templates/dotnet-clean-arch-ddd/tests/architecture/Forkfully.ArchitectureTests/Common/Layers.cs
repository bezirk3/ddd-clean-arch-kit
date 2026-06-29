using System.Reflection;

namespace Forkfully.ArchitectureTests.Common;

// The architecture under test, as namespaces + the assembly that backs each layer.
// A type from every layer anchors its assembly so NetArchTest can inspect it.
internal static class Layers
{
    public const string Domain = "Forkfully.Domain";
    public const string Application = "Forkfully.Application";
    public const string Infrastructure = "Forkfully.Infrastructure";
    public const string Api = "Forkfully.Api";
    public const string Contracts = "Forkfully.Contracts";

    public static readonly Assembly DomainAssembly =
        typeof(Forkfully.Domain.Common.Models.IDomainEvent).Assembly;

    public static readonly Assembly ApplicationAssembly =
        typeof(Forkfully.Application.DependencyInjection).Assembly;

    public static readonly Assembly InfrastructureAssembly =
        typeof(Forkfully.Infrastructure.DependencyInjection).Assembly;

    public static readonly Assembly ApiAssembly =
        typeof(Forkfully.Api.DependencyInjection).Assembly;

    public static readonly Assembly ContractsAssembly =
        typeof(Forkfully.Contracts.Authentication.RegisterRequest).Assembly;

    // Every first-party assembly (used by the "no dependency on X" sweeps).
    public static readonly Assembly[] All =
    [
        DomainAssembly,
        ApplicationAssembly,
        InfrastructureAssembly,
        ApiAssembly,
        ContractsAssembly,
    ];
}
