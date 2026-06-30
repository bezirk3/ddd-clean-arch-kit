using Forkfully.Domain.Bill;
using Forkfully.Domain.Dinner;
using Forkfully.Domain.Guest;
using Forkfully.Domain.Host;
using Forkfully.Domain.Menu;
using Forkfully.Domain.MenuReview;
using Forkfully.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Common.Interfaces;

// The persistence seam, replacing the per-aggregate repositories. EF Core's DbContext
// already *is* the repository + unit-of-work pattern, so the Application talks to it
// directly through this interface (implemented by ApplicationDbContext in
// Infrastructure). The trade-off: the Application now references EF Core and exposes
// IQueryable/DbSet — it gives up persistence ignorance, but keeps the dependency
// pointing inward (the interface lives here, not the concrete context).
public interface IApplicationDbContext
{
    DbSet<User> Users { get; }

    DbSet<Menu> Menus { get; }

    DbSet<Host> Hosts { get; }

    DbSet<Guest> Guests { get; }

    DbSet<Dinner> Dinners { get; }

    DbSet<Bill> Bills { get; }

    DbSet<MenuReview> MenuReviews { get; }

    // The DbContext is the unit of work: one SaveChangesAsync commits the whole change
    // set. Async because persistence is I/O — don't block a thread on the round-trip.
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
