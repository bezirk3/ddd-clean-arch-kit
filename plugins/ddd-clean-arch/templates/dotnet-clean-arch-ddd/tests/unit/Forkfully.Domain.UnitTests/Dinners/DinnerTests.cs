using Forkfully.Domain.Common.Errors;
using Forkfully.Domain.Common.ValueObjects;
using Forkfully.Domain.Dinner.Events;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu.ValueObjects;
using ErrorOr;
using DomainDinner = Forkfully.Domain.Dinner.Dinner;

namespace Forkfully.Domain.UnitTests.Dinners;

public class DinnerTests
{
    private static readonly DateTime Start = new(2030, 1, 1, 18, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Create_WhenValid_ReturnsDinnerAndRaisesDinnerCreated()
    {
        var result = CreateDinner(start: Start, end: Start.AddHours(3), maxGuestCount: 10);

        Assert.False(result.IsError);
        Assert.Single(result.Value.DomainEvents);
        Assert.IsType<DinnerCreated>(result.Value.DomainEvents[0]);
    }

    [Fact]
    public void Create_WhenEndIsNotAfterStart_ReturnsEndBeforeStart()
    {
        var result = CreateDinner(start: Start, end: Start, maxGuestCount: 10);

        Assert.True(result.IsError);
        Assert.Equal(Errors.Dinner.EndBeforeStart, result.FirstError);
    }

    [Fact]
    public void Create_WhenMaxGuestCountIsNotPositive_ReturnsInvalidGuestCount()
    {
        var result = CreateDinner(start: Start, end: Start.AddHours(3), maxGuestCount: 0);

        Assert.True(result.IsError);
        Assert.Equal(Errors.Dinner.InvalidGuestCount, result.FirstError);
    }

    private static ErrorOr<DomainDinner> CreateDinner(DateTime start, DateTime end, int maxGuestCount) =>
        DomainDinner.Create(
            name: "Sunday Roast",
            description: "A cozy dinner",
            startDateTime: start,
            endDateTime: end,
            maxGuestCount: maxGuestCount,
            price: Price.Create(50m, "USD"),
            hostId: HostId.CreateUnique(),
            menuId: MenuId.CreateUnique(),
            location: Location.Create("Home", "1 Main St", 0, 0));
}
