using Forkfully.Domain.Host;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Hosts.Queries.GetHost;

public record GetHostQuery(string HostId) : IRequest<ErrorOr<Host>>;
