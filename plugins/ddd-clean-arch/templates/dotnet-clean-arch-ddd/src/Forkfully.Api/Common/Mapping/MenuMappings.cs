using Forkfully.Application.Menus.Commands.CreateMenu;
using Forkfully.Contracts.Menus;
using Forkfully.Domain.Menu;

namespace Forkfully.Api.Common.Mapping;

public static class MenuMappings
{
    // HostId comes from the route; the rest from the request. Nested sections/items
    // are projected by hand (this replaces Mapster's nested-config resolution).
    public static CreateMenuCommand ToCommand(this CreateMenuRequest request, string hostId) =>
        new(
            hostId,
            request.Name,
            request.Description,
            request.Sections.Select(section => new MenuSectionCommand(
                section.Name,
                section.Description,
                section.Items
                    .Select(item => new MenuItemCommand(item.Name, item.Description))
                    .ToList())).ToList());

    public static MenuResponse ToResponse(this Menu menu) =>
        new(
            menu.Id.Value.ToString(),
            menu.Name,
            menu.Description,
            (float)menu.AverageRating.Value,
            menu.Sections.Select(section => new MenuSectionResponse(
                section.Id.Value.ToString(),
                section.Name,
                section.Description,
                section.Items.Select(item => new MenuItemResponse(
                    item.Id.Value.ToString(),
                    item.Name,
                    item.Description)).ToList())).ToList(),
            menu.HostId.Value.ToString(),
            menu.DinnerIds.Select(id => id.Value.ToString()).ToList(),
            menu.MenuReviewIds.Select(id => id.Value.ToString()).ToList(),
            menu.CreatedDateTime,
            menu.UpdatedDateTime);
}
