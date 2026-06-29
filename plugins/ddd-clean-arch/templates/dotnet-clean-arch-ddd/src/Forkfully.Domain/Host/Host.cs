using Forkfully.Domain.Common.Models;
using Forkfully.Domain.Common.ValueObjects;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu.ValueObjects;
using Forkfully.Domain.User.ValueObjects;
using ErrorOr;
using DomainDinner = Forkfully.Domain.Dinner.Dinner;
using DomainMenu = Forkfully.Domain.Menu.Menu;

namespace Forkfully.Domain.Host;

// A user acting as a host — the owner of menus and dinners.
public sealed class Host : AggregateRoot<HostId, Guid>
{
    private readonly List<MenuId> _menuIds = new();
    private readonly List<DinnerId> _dinnerIds = new();

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string ProfileImage { get; private set; }
    public AverageRating AverageRating { get; private set; }
    public UserId UserId { get; private set; }

    public IReadOnlyList<MenuId> MenuIds => _menuIds.AsReadOnly();
    public IReadOnlyList<DinnerId> DinnerIds => _dinnerIds.AsReadOnly();

    private Host(
        HostId hostId,
        string firstName,
        string lastName,
        string profileImage,
        AverageRating averageRating,
        UserId userId)
        : base(hostId)
    {
        FirstName = firstName;
        LastName = lastName;
        ProfileImage = profileImage;
        AverageRating = averageRating;
        UserId = userId;
    }

#pragma warning disable CS8618
    private Host()
    {
    }
#pragma warning restore CS8618

    public static ErrorOr<Host> Create(UserId userId, string firstName, string lastName, string profileImage)
    {
        return new Host(
            HostId.CreateUnique(),
            firstName,
            lastName,
            profileImage,
            AverageRating.CreateNew(),
            userId);
    }

    // Cross-aggregate references are by id (to the root only).
    public void AddMenu(DomainMenu menu) => _menuIds.Add(MenuId.Create(menu.Id.Value));

    public void AddDinner(DomainDinner dinner) => _dinnerIds.Add(DinnerId.Create(dinner.Id.Value));
}
