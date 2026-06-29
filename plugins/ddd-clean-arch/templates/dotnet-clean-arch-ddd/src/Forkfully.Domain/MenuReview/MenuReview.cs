using Forkfully.Domain.Common.Errors;
using Forkfully.Domain.Common.Models;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu.ValueObjects;
using Forkfully.Domain.MenuReview.ValueObjects;
using ErrorOr;

namespace Forkfully.Domain.MenuReview;

// A guest's review of a menu. Reviews attach to the Menu (not the Dinner), so they
// accumulate across every dinner run on that menu.
public sealed class MenuReview : AggregateRoot<MenuReviewId, Guid>
{
    public int Rating { get; private set; }
    public string Comment { get; private set; }
    public HostId HostId { get; private set; }
    public MenuId MenuId { get; private set; }
    public GuestId GuestId { get; private set; }
    public DinnerId DinnerId { get; private set; }

    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private MenuReview(
        MenuReviewId menuReviewId,
        int rating,
        string comment,
        HostId hostId,
        MenuId menuId,
        GuestId guestId,
        DinnerId dinnerId)
        : base(menuReviewId)
    {
        Rating = rating;
        Comment = comment;
        HostId = hostId;
        MenuId = menuId;
        GuestId = guestId;
        DinnerId = dinnerId;
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

#pragma warning disable CS8618
    private MenuReview()
    {
    }
#pragma warning restore CS8618

    public static ErrorOr<MenuReview> Create(
        HostId hostId,
        MenuId menuId,
        GuestId guestId,
        DinnerId dinnerId,
        int rating,
        string comment)
    {
        if (rating is < 1 or > 5)
        {
            return Errors.MenuReview.InvalidRating;
        }

        return new MenuReview(
            MenuReviewId.CreateUnique(),
            rating,
            comment,
            hostId,
            menuId,
            guestId,
            dinnerId);
    }
}
