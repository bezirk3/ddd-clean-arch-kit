using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Menus.Queries.ListMenus;

public class ListMenusQueryHandler : IRequestHandler<ListMenusQuery, ErrorOr<List<Menu>>>
{
    private readonly IApplicationDbContext _context;

    public ListMenusQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<List<Menu>>> Handle(ListMenusQuery query, CancellationToken cancellationToken)
    {
        var hostId = HostId.Create(Guid.Parse(query.HostId));

        return await _context.Menus
            .Where(menu => menu.HostId == hostId)
            .ToListAsync(cancellationToken);
    }
}
