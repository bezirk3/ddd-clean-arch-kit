using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Dinner.Events;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Menu.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Dinners.Events;

// Real cross-aggregate handler: when a Dinner is created, record its id on the Menu it
// runs against. Published by the SaveChanges interceptor; the interceptor clears events
// before dispatch, so the second SaveChangesAsync here does not re-enter recursively.
public class DinnerCreatedEventHandler : IDomainEventHandler<DinnerCreated>
{
    private readonly IApplicationDbContext _context;

    public DinnerCreatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DinnerCreated domainEvent, CancellationToken cancellationToken)
    {
        var menuId = MenuId.Create(domainEvent.Dinner.MenuId.Value);

        var menu = await _context.Menus
            .SingleOrDefaultAsync(m => m.Id == menuId, cancellationToken);

        if (menu is null)
        {
            return;
        }

        menu.AddDinnerId(DinnerId.Create(domainEvent.Dinner.Id.Value));
        await _context.SaveChangesAsync(cancellationToken);
    }
}
