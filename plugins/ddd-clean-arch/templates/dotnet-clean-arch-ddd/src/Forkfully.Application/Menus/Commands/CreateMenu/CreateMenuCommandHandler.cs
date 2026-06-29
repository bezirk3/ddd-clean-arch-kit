using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu;
using Forkfully.Domain.Menu.Entities;
using ErrorOr;

namespace Forkfully.Application.Menus.Commands.CreateMenu;

public class CreateMenuCommandHandler
    : IRequestHandler<CreateMenuCommand, ErrorOr<Menu>>
{
    private readonly IApplicationDbContext _context;

    public CreateMenuCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Menu>> Handle(
        CreateMenuCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Create menu (the factory returns ErrorOr so it can enforce invariants)
        var result = Menu.Create(
            hostId: HostId.Create(Guid.Parse(command.HostId)),
            name: command.Name,
            description: command.Description,
            sections: command.Sections.ConvertAll(section => MenuSection.Create(
                name: section.Name,
                description: section.Description,
                items: section.Items.ConvertAll(item => MenuItem.Create(
                    name: item.Name,
                    description: item.Description)))));

        if (result.IsError)
        {
            return result.Errors;
        }

        // 2. Persist menu (raises MenuCreated, published by the SaveChanges interceptor)
        var menu = result.Value;
        _context.Menus.Add(menu);
        await _context.SaveChangesAsync(cancellationToken);

        // 3. Return menu
        return menu;
    }
}
