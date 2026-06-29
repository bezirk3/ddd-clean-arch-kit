namespace Forkfully.Application.Common.Messaging;

// Handles a single request type. Injected directly into the endpoint that needs it
// (no ISender, no mediator) — so the call is a plain, steppable method call. Cross-
// cutting concerns are added by DI-resolved decorators that also implement this
// interface (see ValidationDecorator / LoggingDecorator), not a hidden pipeline.
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
