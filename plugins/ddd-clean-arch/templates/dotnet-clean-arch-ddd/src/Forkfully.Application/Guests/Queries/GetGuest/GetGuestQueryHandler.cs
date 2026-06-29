using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Guest;
using Forkfully.Domain.Guest.ValueObjects;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Guests.Queries.GetGuest;

public class GetGuestQueryHandler : IRequestHandler<GetGuestQuery, ErrorOr<Guest>>
{
    private readonly IApplicationDbContext _context;

    public GetGuestQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Guest>> Handle(GetGuestQuery query, CancellationToken cancellationToken)
    {
        var guestId = GuestId.Create(Guid.Parse(query.GuestId));
        var guest = await _context.Guests.SingleOrDefaultAsync(g => g.Id == guestId, cancellationToken);

        return guest is null
            ? Error.NotFound("Guest.NotFound", "Guest not found.")
            : guest;
    }
}
