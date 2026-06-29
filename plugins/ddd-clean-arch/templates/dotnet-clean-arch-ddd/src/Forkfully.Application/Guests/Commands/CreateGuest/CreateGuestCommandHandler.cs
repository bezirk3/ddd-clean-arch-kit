using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Guest;
using Forkfully.Domain.User.ValueObjects;
using ErrorOr;

namespace Forkfully.Application.Guests.Commands.CreateGuest;

public class CreateGuestCommandHandler : IRequestHandler<CreateGuestCommand, ErrorOr<Guest>>
{
    private readonly IApplicationDbContext _context;

    public CreateGuestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Guest>> Handle(CreateGuestCommand command, CancellationToken cancellationToken)
    {
        var result = Guest.Create(
            UserId.Create(Guid.Parse(command.UserId)),
            command.FirstName,
            command.LastName,
            command.ProfileImage);

        if (result.IsError)
        {
            return result.Errors;
        }

        var guest = result.Value;
        _context.Guests.Add(guest);
        await _context.SaveChangesAsync(cancellationToken);

        return guest;
    }
}
