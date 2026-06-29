using Forkfully.Application.Common.Messaging;
using ErrorOr;
using FluentValidation;

namespace Forkfully.Application.Common.Behaviors;

// Cross-cutting validation as a DECORATOR rather than a MediatR pipeline behavior.
// It implements the same IRequestHandler<TRequest, TResponse> it wraps, runs the
// FluentValidation validator first, and only calls the inner handler when the
// request is valid. Wired up in MessagingRegistration.AddRequestHandler — the inner
// handler never knows it's decorated. Same behavior the ValidationBehavior had.
public class ValidationDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IRequestHandler<TRequest, TResponse> _inner;
    private readonly IValidator<TRequest>? _validator;

    public ValidationDecorator(
        IRequestHandler<TRequest, TResponse> inner,
        IValidator<TRequest>? validator = null)
    {
        _inner = inner;
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        // No validator registered for this request → straight to the handler.
        if (_validator is null)
        {
            return await _inner.Handle(request, cancellationToken);
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await _inner.Handle(request, cancellationToken);
        }

        var errors = validationResult.Errors
            .ConvertAll(validationFailure => Error.Validation(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage));

        // The IErrorOr constraint guarantees TResponse is an ErrorOr<T> with an
        // implicit conversion from List<Error>; dynamic resolves it at runtime.
        return (dynamic)errors;
    }
}
