using Forkfully.Application.Menus.Commands.CreateMenu;
using Forkfully.Application.UnitTests.TestUtils;

namespace Forkfully.Application.UnitTests.Menus.Commands.CreateMenu.TestUtils;

public static class CreateMenuCommandUtils
{
    // Inner objects are passed as optional parameters — never "logic" parameters
    // (counts/bools). Callers compose with CreateSectionsCommand / CreateItemsCommand.
    public static CreateMenuCommand CreateCommand(
        List<MenuSectionCommand>? sections = null)
    {
        return new CreateMenuCommand(
            Constants.Host.Id.Value.ToString(),
            Constants.Menu.Name,
            Constants.Menu.Description,
            sections ?? CreateSectionsCommand());
    }

    public static List<MenuSectionCommand> CreateSectionsCommand(
        int sectionCount = 1,
        List<MenuItemCommand>? items = null)
    {
        return Enumerable.Range(0, sectionCount)
            .Select(index => new MenuSectionCommand(
                Constants.Menu.SectionNameFromIndex(index),
                Constants.Menu.SectionDescriptionFromIndex(index),
                items ?? CreateItemsCommand()))
            .ToList();
    }

    public static List<MenuItemCommand> CreateItemsCommand(
        int itemCount = 1)
    {
        return Enumerable.Range(0, itemCount)
            .Select(index => new MenuItemCommand(
                Constants.Menu.ItemNameFromIndex(index),
                Constants.Menu.ItemDescriptionFromIndex(index)))
            .ToList();
    }
}
