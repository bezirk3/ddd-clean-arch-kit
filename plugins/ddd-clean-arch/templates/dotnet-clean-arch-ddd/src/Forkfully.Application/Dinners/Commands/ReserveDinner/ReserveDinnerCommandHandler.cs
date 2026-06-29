using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Bill;
using Forkfully.Domain.Bill.ValueObjects;
using Forkfully.Domain.Dinner;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.ValueObjects;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Dinners.Commands.ReserveDinner;

// Cross-aggregate flow expressed as a railway (ROP) over ErrorOr's own operators:
//   load dinner -> reserve a spot (builds the Bill, enforces the capacity invariant)
//   -> persist -> return the dinner.
// `.Then` is bind (short-circuits on error), `.ThenDoAsync` is an async tap (side
// effect that can't fail). No TryCatch step: unexpected exceptions stay exceptions and
// fall to the global handler (ADR-0021); only expected failures travel as errors. The
// final `.Match` lives in the endpoint. Persistence is async (ADR-0045).
public class ReserveDinnerCommandHandler : IRequestHandler<ReserveDinnerCommand, ErrorOr<Dinner>>
{
    private readonly IApplicationDbContext _context;

    public ReserveDinnerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Dinner>> Handle(ReserveDinnerCommand command, CancellationToken cancellationToken)
    {
        var dinnerId = DinnerId.Create(Guid.Parse(command.DinnerId));
        var guestId = GuestId.Create(Guid.Parse(command.GuestId));

        return await GetDinnerAsync(dinnerId, cancellationToken)
            .Then(dinner => ReserveSpot(dinner, dinnerId, guestId, command.GuestCount))
            .ThenDoAsync(reserved => PersistAsync(reserved.Bill, cancellationToken))
            .Then(reserved => reserved.Dinner);
    }

    private async Task<ErrorOr<Dinner>> GetDinnerAsync(DinnerId dinnerId, CancellationToken cancellationToken)
    {
        // Loaded (and tracked) by the context, so the reservation added below is saved
        // alongside the bill in a single SaveChangesAsync.
        var dinner = await _context.Dinners.SingleOrDefaultAsync(d => d.Id == dinnerId, cancellationToken);

        return dinner is null
            ? Error.NotFound("Dinner.NotFound", "Dinner not found.")
            : dinner;
    }

    // Build the bill up front so the reservation can reference it, then reserve the
    // spot; the capacity invariant fails here and short-circuits the railway, so the
    // bill below is never persisted. Carries (dinner, bill) forward as a tuple.
    private static ErrorOr<(Dinner Dinner, Bill Bill)> ReserveSpot(
        Dinner dinner,
        DinnerId dinnerId,
        GuestId guestId,
        int guestCount)
    {
        var bill = Bill.Create(dinnerId, guestId, dinner.Price);

        return dinner
            .ReserveSpot(guestId, guestCount, BillId.Create(bill.Id.Value))
            .Then<(Dinner, Bill)>(_ => (dinner, bill));
    }

    private async Task PersistAsync(Bill bill, CancellationToken cancellationToken)
    {
        // The dinner is already tracked, so this one SaveChangesAsync commits both the
        // new bill and the dinner's new reservation atomically — the DbContext is the
        // unit of work.
        _context.Bills.Add(bill);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
