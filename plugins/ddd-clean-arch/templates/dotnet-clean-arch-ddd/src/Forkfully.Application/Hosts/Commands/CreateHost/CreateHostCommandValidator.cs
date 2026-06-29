using FluentValidation;

namespace Forkfully.Application.Hosts.Commands.CreateHost;

public class CreateHostCommandValidator : AbstractValidator<CreateHostCommand>
{
    public CreateHostCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
