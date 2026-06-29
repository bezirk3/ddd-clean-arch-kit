using Forkfully.Application.Dinners.Commands.ReserveDinner;

namespace Forkfully.Application.UnitTests.Dinners.Commands.ReserveDinner;

public class ReserveDinnerCommandValidatorTests
{
    private readonly ReserveDinnerCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenAllFieldsAreValid_Passes()
    {
        var command = new ReserveDinnerCommand(
            DinnerId: Guid.NewGuid().ToString(),
            GuestId: Guid.NewGuid().ToString(),
            GuestCount: 2);

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("", "guest-id", 2)]      // missing dinner id
    [InlineData("dinner-id", "", 2)]     // missing guest id
    [InlineData("dinner-id", "guest-id", 0)]   // non-positive guest count
    [InlineData("dinner-id", "guest-id", -1)]
    public void Validate_WhenAFieldIsInvalid_Fails(string dinnerId, string guestId, int guestCount)
    {
        var command = new ReserveDinnerCommand(dinnerId, guestId, guestCount);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
    }
}
