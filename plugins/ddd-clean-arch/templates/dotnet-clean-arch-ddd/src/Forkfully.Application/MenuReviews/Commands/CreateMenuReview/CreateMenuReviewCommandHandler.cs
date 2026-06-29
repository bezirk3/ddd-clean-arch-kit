using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu.ValueObjects;
using Forkfully.Domain.MenuReview;
using ErrorOr;

namespace Forkfully.Application.MenuReviews.Commands.CreateMenuReview;

public class CreateMenuReviewCommandHandler
    : IRequestHandler<CreateMenuReviewCommand, ErrorOr<MenuReview>>
{
    private readonly IApplicationDbContext _context;

    public CreateMenuReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<MenuReview>> Handle(
        CreateMenuReviewCommand command,
        CancellationToken cancellationToken)
    {
        var result = MenuReview.Create(
            HostId.Create(Guid.Parse(command.HostId)),
            MenuId.Create(Guid.Parse(command.MenuId)),
            GuestId.Create(Guid.Parse(command.GuestId)),
            DinnerId.Create(Guid.Parse(command.DinnerId)),
            command.Rating,
            command.Comment);

        if (result.IsError)
        {
            return result.Errors;
        }

        var menuReview = result.Value;
        _context.MenuReviews.Add(menuReview);
        await _context.SaveChangesAsync(cancellationToken);

        return menuReview;
    }
}
