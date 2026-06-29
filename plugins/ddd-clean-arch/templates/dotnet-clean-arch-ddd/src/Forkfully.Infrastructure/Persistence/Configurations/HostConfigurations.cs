using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Host;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu.ValueObjects;
using Forkfully.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forkfully.Infrastructure.Persistence.Configurations;

public class HostConfigurations : IEntityTypeConfiguration<Host>
{
    public void Configure(EntityTypeBuilder<Host> builder)
    {
        builder.ToTable("Hosts");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => HostId.Create(value));

        builder.Property(h => h.UserId)
            .HasConversion(id => id.Value, value => UserId.Create(value));

        builder.Property(h => h.FirstName).HasMaxLength(100);
        builder.Property(h => h.LastName).HasMaxLength(100);

        builder.OwnsOne(h => h.AverageRating);

        builder.OwnsMany(h => h.MenuIds, mb =>
        {
            mb.ToTable("HostMenuIds");
            mb.WithOwner().HasForeignKey("HostId");
            mb.HasKey("Id");
            mb.Property(menuId => menuId.Value).HasColumnName("MenuId").ValueGeneratedNever();
        });
        builder.Metadata.FindNavigation(nameof(Host.MenuIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(h => h.DinnerIds, db =>
        {
            db.ToTable("HostDinnerIds");
            db.WithOwner().HasForeignKey("HostId");
            db.HasKey("Id");
            db.Property(dinnerId => dinnerId.Value).HasColumnName("DinnerId").ValueGeneratedNever();
        });
        builder.Metadata.FindNavigation(nameof(Host.DinnerIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
