using Aspire.Hosting.ApplicationModel;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Forkfully_Api>("api")
    .WithUrlForEndpoint("http", _ => new ResourceUrlAnnotation
    {
        Url = "/scalar/v1",
        DisplayText = "Scalar",
    });

await builder
    .Build()
    .RunAsync();
