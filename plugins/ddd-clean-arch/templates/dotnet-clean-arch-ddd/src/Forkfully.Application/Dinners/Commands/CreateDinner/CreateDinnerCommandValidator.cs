using FluentValidation;

namespace Forkfully.Application.Dinners.Commands.CreateDinner;

public class CreateDinnerCommandValidator : AbstractValidator<CreateDinnerCommand>
{
    public CreateDinnerCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.HostId).NotEmpty();
        RuleFor(x => x.MenuId).NotEmpty();
        RuleFor(x => x.MaxGuestCount).GreaterThan(0);
        RuleFor(x => x.EndDateTime).GreaterThan(x => x.StartDateTime);
    }
}
