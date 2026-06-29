using Forkfully.Domain.Common.Models;
using Forkfully.Domain.Common.ValueObjects;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu.Entities;
using Forkfully.Domain.Menu.Events;
using Forkfully.Domain.Menu.ValueObjects;
using Forkfully.Domain.MenuReview.ValueObjects;
using ErrorOr;

namespace Forkfully.Domain.Menu;

public sealed class Menu : AggregateRoot<MenuId, Guid>
{
    private readonly List<MenuSection> _sections = new();
    private readonly List<DinnerId> _dinnerIds = new();
    private readonly List<MenuReviewId> _menuReviewIds = new();

    public string Name { get; private set; }
    public string Description { get; private set; }
    public AverageRating AverageRating { get; private set; }
    public HostId HostId { get; private set; }

    public IReadOnlyList<MenuSection> Sections => _sections.AsReadOnly();
    public IReadOnlyList<DinnerId> DinnerIds => _dinnerIds.AsReadOnly();
    public IReadOnlyList<MenuReviewId> MenuReviewIds => _menuReviewIds.AsReadOnly();

    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private Menu(
        MenuId menuId,
        HostId hostId,
        string name,
        string description,
        AverageRating averageRating,
        List<MenuSection> sections)
        : base(menuId)
    {
        HostId = hostId;
        Name = name;
        Description = description;
        AverageRating = averageRating;
        _sections = sections;
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

#pragma warning disable CS8618
    // Parameterless constructor for EF Core.
    private Menu()
    {
    }
#pragma warning restore CS8618

    public static ErrorOr<Menu> Create(
        HostId hostId,
        string name,
        string description,
        List<MenuSection>? sections = null)
    {
        var menu = new Menu(
            MenuId.CreateUnique(),
            hostId,
            name,
            description,
            AverageRating.CreateNew(),
            sections ?? new());

        menu.AddDomainEvent(new MenuCreated(menu));

        return menu;
    }

    // A Dinner records itself here when created (DinnerCreated → DinnerCreatedEventHandler),
    // so a menu knows which dinners have been run against it.
    public void AddDinnerId(DinnerId dinnerId)
    {
        if (_dinnerIds.Contains(dinnerId))
        {
            return;
        }

        _dinnerIds.Add(dinnerId);
        UpdatedDateTime = DateTime.UtcNow;
    }
}
