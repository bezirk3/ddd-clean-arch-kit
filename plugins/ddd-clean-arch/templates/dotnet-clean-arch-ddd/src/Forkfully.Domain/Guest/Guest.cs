using Forkfully.Domain.Bill.ValueObjects;
using Forkfully.Domain.Common.Models;
using Forkfully.Domain.Common.ValueObjects;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.Entities;
using Forkfully.Domain.Guest.ValueObjects;
using Forkfully.Domain.MenuReview.ValueObjects;
using Forkfully.Domain.User.ValueObjects;
using ErrorOr;
using DomainBill = Forkfully.Domain.Bill.Bill;
using DomainMenuReview = Forkfully.Domain.MenuReview.MenuReview;

namespace Forkfully.Domain.Guest;

// A user acting as a guest — reserves spots, pays bills, reviews menus, gets rated.
public sealed class Guest : AggregateRoot<GuestId, Guid>
{
    private readonly List<DinnerId> _upcomingDinnerIds = new();
    private readonly List<DinnerId> _pastDinnerIds = new();
    private readonly List<BillId> _billIds = new();
    private readonly List<MenuReviewId> _menuReviewIds = new();
    private readonly List<GuestRating> _ratings = new();

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string ProfileImage { get; private set; }
    public AverageRating AverageRating { get; private set; }
    public UserId UserId { get; private set; }

    public IReadOnlyList<DinnerId> UpcomingDinnerIds => _upcomingDinnerIds.AsReadOnly();
    public IReadOnlyList<DinnerId> PastDinnerIds => _pastDinnerIds.AsReadOnly();
    public IReadOnlyList<BillId> BillIds => _billIds.AsReadOnly();
    public IReadOnlyList<MenuReviewId> MenuReviewIds => _menuReviewIds.AsReadOnly();
    public IReadOnlyList<GuestRating> Ratings => _ratings.AsReadOnly();

    private Guest(
        GuestId guestId,
        string firstName,
        string lastName,
        string profileImage,
        AverageRating averageRating,
        UserId userId)
        : base(guestId)
    {
        FirstName = firstName;
        LastName = lastName;
        ProfileImage = profileImage;
        AverageRating = averageRating;
        UserId = userId;
    }

#pragma warning disable CS8618
    private Guest()
    {
    }
#pragma warning restore CS8618

    public static ErrorOr<Guest> Create(UserId userId, string firstName, string lastName, string profileImage)
    {
        return new Guest(
            GuestId.CreateUnique(),
            firstName,
            lastName,
            profileImage,
            AverageRating.CreateNew(),
            userId);
    }

    public void AddUpcomingDinner(DinnerId dinnerId) => _upcomingDinnerIds.Add(dinnerId);

    public void AddBill(DomainBill bill) => _billIds.Add(BillId.Create(bill.Id.Value));

    public void AddMenuReview(DomainMenuReview review) => _menuReviewIds.Add(MenuReviewId.Create(review.Id.Value));

    public void AddRating(GuestRating rating) => _ratings.Add(rating);
}
