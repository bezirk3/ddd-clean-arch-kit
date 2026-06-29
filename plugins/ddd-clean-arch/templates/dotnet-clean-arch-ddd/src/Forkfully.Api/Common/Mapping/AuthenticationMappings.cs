using Forkfully.Application.Authentication.Commands.Register;
using Forkfully.Application.Authentication.Common;
using Forkfully.Application.Authentication.Queries.Login;
using Forkfully.Contracts.Authentication;

namespace Forkfully.Api.Common.Mapping;

// Hand-written mapping (replaces the Mapster IRegister config). Lives in the Api
// layer because it bridges Contracts ↔ Application; explicit, debuggable, no library.
public static class AuthenticationMappings
{
    public static RegisterCommand ToCommand(this RegisterRequest request) =>
        new(request.FirstName, request.LastName, request.Email, request.Password);

    public static LoginQuery ToQuery(this LoginRequest request) =>
        new(request.Email, request.Password);

    public static AuthenticationResponse ToResponse(this AuthenticationResult result) =>
        new(
            result.User.Id.Value,
            result.User.FirstName,
            result.User.LastName,
            result.User.Email,
            result.Token);
}
