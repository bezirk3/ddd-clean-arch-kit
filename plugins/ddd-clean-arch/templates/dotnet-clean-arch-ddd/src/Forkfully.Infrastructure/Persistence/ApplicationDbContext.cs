using Forkfully.Application.Common.Interfaces;
using Forkfully.Domain.Common.Models;
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
    // Add a DbSet per aggregate you introduce; its IEntityTypeConfiguration in
    // Persistence/Configurations is picked up by ApplyConfigurationsFromAssembly below.

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
