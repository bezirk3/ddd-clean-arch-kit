using Forkfully.Application.Authentication.Common;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
