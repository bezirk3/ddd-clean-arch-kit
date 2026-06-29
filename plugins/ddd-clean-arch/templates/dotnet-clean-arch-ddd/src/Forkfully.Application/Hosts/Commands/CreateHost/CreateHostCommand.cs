using Forkfully.Domain.Host;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Hosts.Commands.CreateHost;

public record CreateHostCommand(
    string UserId,
    string FirstName,
    string LastName,
    string ProfileImage) : IRequest<ErrorOr<Host>>;
