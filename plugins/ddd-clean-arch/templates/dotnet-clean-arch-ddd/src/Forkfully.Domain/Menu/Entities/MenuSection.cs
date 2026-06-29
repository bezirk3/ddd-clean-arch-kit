using Forkfully.Domain.Common.Models;
using Forkfully.Domain.Menu.ValueObjects;

namespace Forkfully.Domain.Menu.Entities;

public sealed class MenuSection : Entity<MenuSectionId>
{
    private readonly List<MenuItem> _items = new();

    public string Name { get; private set; }
    public string Description { get; private set; }

    // Exposed read-only so callers can't mutate the aggregate's internals.
    public IReadOnlyList<MenuItem> Items => _items.AsReadOnly();

    private MenuSection(
        MenuSectionId menuSectionId,
        string name,
        string description,
        List<MenuItem> items)
        : base(menuSectionId)
    {
        Name = name;
        Description = description;
        _items = items;
    }

#pragma warning disable CS8618
    // Parameterless constructor for EF Core.
    private MenuSection()
    {
    }
#pragma warning restore CS8618

    public static MenuSection Create(
        string name,
        string description,
        List<MenuItem> items)
    {
        return new MenuSection(
            MenuSectionId.CreateUnique(),
            name,
            description,
            items);
    }
}
