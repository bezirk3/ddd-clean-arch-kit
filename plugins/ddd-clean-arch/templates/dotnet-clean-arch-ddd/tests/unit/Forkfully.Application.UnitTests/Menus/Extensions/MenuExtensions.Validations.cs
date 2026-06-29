using Forkfully.Application.Menus.Commands.CreateMenu;
using DomainMenu = Forkfully.Domain.Menu.Menu;

namespace Forkfully.Application.UnitTests.Menus.Extensions;

public static partial class MenuExtensions
{
    public static void ValidateCreatedFrom(this DomainMenu menu, CreateMenuCommand command)
    {
        Assert.Equal(command.Name, menu.Name);
        Assert.Equal(command.Description, menu.Description);
        Assert.Equal(command.HostId, menu.HostId.Value.ToString());

        Assert.Equal(command.Sections.Count, menu.Sections.Count);
        menu.Sections.Zip(command.Sections).ToList().ForEach(sectionPair =>
        {
            var (menuSection, commandSection) = sectionPair;
            Assert.Equal(commandSection.Name, menuSection.Name);
            Assert.Equal(commandSection.Description, menuSection.Description);

            Assert.Equal(commandSection.Items.Count, menuSection.Items.Count);
            menuSection.Items.Zip(commandSection.Items).ToList().ForEach(itemPair =>
            {
                var (menuItem, commandItem) = itemPair;
                Assert.Equal(commandItem.Name, menuItem.Name);
                Assert.Equal(commandItem.Description, menuItem.Description);
            });
        });
    }
}
