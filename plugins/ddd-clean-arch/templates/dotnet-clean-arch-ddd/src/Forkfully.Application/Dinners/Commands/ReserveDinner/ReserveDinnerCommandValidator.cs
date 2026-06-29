using FluentValidation;

namespace Forkfully.Application.Dinners.Commands.ReserveDinner;

public class ReserveDinnerCommandValidator : AbstractValidator<ReserveDinnerCommand>
{
    public ReserveDinnerCommandValidator()
    {
        RuleFor(x => x.DinnerId).NotEmpty();
        RuleFor(x => x.GuestId).NotEmpty();
        RuleFor(x => x.GuestCount).GreaterThan(0);
    }
}
