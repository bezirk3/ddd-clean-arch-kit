using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Menus.Commands.CreateMenu;
using Forkfully.Application.UnitTests.Menus.Commands.CreateMenu.TestUtils;
using Forkfully.Application.UnitTests.Menus.Extensions;
using Microsoft.EntityFrameworkCore;
using Moq;
using DomainMenu = Forkfully.Domain.Menu.Menu;

namespace Forkfully.Application.UnitTests.Menus.Commands.CreateMenu;

public class CreateMenuCommandHandlerTests
{
    private readonly CreateMenuCommandHandler _handler;
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<DbSet<DomainMenu>> _mockMenus;

    public CreateMenuCommandHandlerTests()
    {
        _mockMenus = new Mock<DbSet<DomainMenu>>();
        _mockContext = new Mock<IApplicationDbContext>();
        _mockContext.Setup(context => context.Menus).Returns(_mockMenus.Object);
        _mockContext
            .Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _handler = new CreateMenuCommandHandler(_mockContext.Object);
    }

    [Theory]
    [MemberData(nameof(ValidCreateMenuCommands))]
    public async Task HandleCreateMenuCommand_WhenMenuIsValid_ShouldCreateAndReturnMenu(
        CreateMenuCommand createMenuCommand)
    {
        // Act
        var result = await _handler.Handle(createMenuCommand, default);

        // Assert
        // 1. the correct menu was created from the command
        Assert.False(result.IsError);
        result.Value.ValidateCreatedFrom(createMenuCommand);

        // 2. the menu was added to the context and committed exactly once
        _mockMenus.Verify(menus => menus.Add(result.Value), Times.Once);
        _mockContext.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    public static IEnumerable<object[]> ValidCreateMenuCommands()
    {
        yield return new object[]
        {
            CreateMenuCommandUtils.CreateCommand(),
        };
        yield return new object[]
        {
            CreateMenuCommandUtils.CreateCommand(
                sections: CreateMenuCommandUtils.CreateSectionsCommand(sectionCount: 3)),
        };
        yield return new object[]
        {
            CreateMenuCommandUtils.CreateCommand(
                sections: CreateMenuCommandUtils.CreateSectionsCommand(
                    sectionCount: 3,
                    items: CreateMenuCommandUtils.CreateItemsCommand(itemCount: 3))),
        };
    }
}
