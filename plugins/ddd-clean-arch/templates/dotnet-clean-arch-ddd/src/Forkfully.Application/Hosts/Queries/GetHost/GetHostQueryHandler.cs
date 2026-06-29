using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Host;
using Forkfully.Domain.Host.ValueObjects;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Hosts.Queries.GetHost;

public class GetHostQueryHandler : IRequestHandler<GetHostQuery, ErrorOr<Host>>
{
    private readonly IApplicationDbContext _context;

    public GetHostQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Host>> Handle(GetHostQuery query, CancellationToken cancellationToken)
    {
        var hostId = HostId.Create(Guid.Parse(query.HostId));
        var host = await _context.Hosts.SingleOrDefaultAsync(h => h.Id == hostId, cancellationToken);

        return host is null
            ? Error.NotFound("Host.NotFound", "Host not found.")
            : host;
    }
}
