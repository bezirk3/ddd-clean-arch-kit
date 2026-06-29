using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Menu;
using Forkfully.Domain.Menu.ValueObjects;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Menus.Queries.GetMenu;

public class GetMenuQueryHandler : IRequestHandler<GetMenuQuery, ErrorOr<Menu>>
{
    private readonly IApplicationDbContext _context;

    public GetMenuQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Menu>> Handle(GetMenuQuery query, CancellationToken cancellationToken)
    {
        var menuId = MenuId.Create(Guid.Parse(query.MenuId));
        var menu = await _context.Menus.SingleOrDefaultAsync(m => m.Id == menuId, cancellationToken);

        return menu is null
            ? Error.NotFound("Menu.NotFound", "Menu not found.")
            : menu;
    }
}
