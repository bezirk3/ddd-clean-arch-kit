using Forkfully.Application.Authentication.Common;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
