using Forkfully.Domain.Common.Errors;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu.ValueObjects;
using ErrorOr;
using DomainMenuReview = Forkfully.Domain.MenuReview.MenuReview;

namespace Forkfully.Domain.UnitTests.MenuReviews;

public class MenuReviewTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void Create_WhenRatingIsOutOfRange_ReturnsInvalidRating(int rating)
    {
        var result = CreateReview(rating);

        Assert.True(result.IsError);
        Assert.Equal(Errors.MenuReview.InvalidRating, result.FirstError);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public void Create_WhenRatingIsInRange_ReturnsReview(int rating)
    {
        var result = CreateReview(rating);

        Assert.False(result.IsError);
        Assert.Equal(rating, result.Value.Rating);
    }

    private static ErrorOr<DomainMenuReview> CreateReview(int rating) =>
        DomainMenuReview.Create(
            hostId: HostId.CreateUnique(),
            menuId: MenuId.CreateUnique(),
            guestId: GuestId.CreateUnique(),
            dinnerId: DinnerId.CreateUnique(),
            rating: rating,
            comment: "Great dinner!");
}
