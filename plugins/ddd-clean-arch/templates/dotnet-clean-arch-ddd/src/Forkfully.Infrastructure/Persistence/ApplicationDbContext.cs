using Forkfully.Application.Common.Interfaces;
using Forkfully.Domain.Bill;
using Forkfully.Domain.Common.Models;
using Forkfully.Domain.Dinner;
using Forkfully.Domain.Guest;
using Forkfully.Domain.Host;
using Forkfully.Domain.Menu;
using Forkfully.Domain.MenuReview;
using Forkfully.Domain.User;
using Forkfully.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Infrastructure.Persistence;

// The DbContext is the unit of work; it implements IApplicationDbContext so the
// Application can use it directly (no per-aggregate repositories) — ADR-0044.
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        PublishDomainEventsInterceptor publishDomainEventsInterceptor)
        : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Menu> Menus { get; set; } = null!;
    public DbSet<Host> Hosts { get; set; } = null!;
    public DbSet<Guest> Guests { get; set; } = null!;
    public DbSet<Dinner> Dinners { get; set; } = null!;
    public DbSet<Bill> Bills { get; set; } = null!;
    public DbSet<MenuReview> MenuReviews { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()    // domain events are never persisted
            .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);

        base.OnConfiguring(optionsBuilder);
    }
}
