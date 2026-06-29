using Forkfully.Application.Common.Messaging;
using Microsoft.Extensions.Logging;

namespace Forkfully.Application.Common.Behaviors;

// The canonical cross-cutting concern, as a decorator: logs around every handled
// request (start / success / failure). Stacks with ValidationDecorator — this one
// is registered as the OUTERMOST wrapper, so it times and reports the whole thing,
// validation included. Same idea as a MediatR LoggingBehavior, but it's a plain
// object you can step into.
public class LoggingDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _inner;
    private readonly ILogger<LoggingDecorator<TRequest, TResponse>> _logger;

    public LoggingDecorator(
        IRequestHandler<TRequest, TResponse> inner,
        ILogger<LoggingDecorator<TRequest, TResponse>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling {Request}", requestName);

        try
        {
            var response = await _inner.Handle(request, cancellationToken);
            _logger.LogInformation("Handled {Request}", requestName);
            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling {Request}", requestName);
            throw;
        }
    }
}
