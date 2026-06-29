namespace Forkfully.Application.Common.Messaging;

// Marker for a command or query that produces a TResponse. Same shape as MediatR's
// IRequest<T>, but it's ours — no package, no hidden dispatch. A request is handled
// by exactly one IRequestHandler<TRequest, TResponse>.
public interface IRequest<TResponse>;
