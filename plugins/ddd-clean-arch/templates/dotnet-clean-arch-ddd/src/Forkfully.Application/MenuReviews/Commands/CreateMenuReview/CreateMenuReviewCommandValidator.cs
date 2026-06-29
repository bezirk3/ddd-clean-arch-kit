using FluentValidation;

namespace Forkfully.Application.MenuReviews.Commands.CreateMenuReview;

public class CreateMenuReviewCommandValidator : AbstractValidator<CreateMenuReviewCommand>
{
    public CreateMenuReviewCommandValidator()
    {
        RuleFor(x => x.HostId).NotEmpty();
        RuleFor(x => x.MenuId).NotEmpty();
        RuleFor(x => x.GuestId).NotEmpty();
        RuleFor(x => x.DinnerId).NotEmpty();
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
    }
}
