using Forkfully.Domain.Bill.ValueObjects;
using Forkfully.Domain.Dinner;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forkfully.Infrastructure.Persistence.Configurations;

public class DinnerConfigurations : IEntityTypeConfiguration<Dinner>
{
    public void Configure(EntityTypeBuilder<Dinner> builder)
    {
        builder.ToTable("Dinners");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => DinnerId.Create(value));

        builder.Property(d => d.Name).HasMaxLength(100);
        builder.Property(d => d.Description).HasMaxLength(1000);

        // Enum stored as its name for readability.
        builder.Property(d => d.Status).HasConversion<string>().HasMaxLength(20);

        builder.Property(d => d.HostId)
            .HasConversion(id => id.Value, value => HostId.Create(value));
        builder.Property(d => d.MenuId)
            .HasConversion(id => id.Value, value => MenuId.Create(value));

        // Value objects → table-split into the Dinners table.
        builder.OwnsOne(d => d.Price, priceBuilder =>
            priceBuilder.Property(price => price.Amount).HasPrecision(18, 2));
        builder.OwnsOne(d => d.Location);

        builder.OwnsMany(d => d.Reservations, rb =>
        {
            rb.ToTable("Reservations");
            rb.WithOwner().HasForeignKey("DinnerId");
            rb.HasKey("Id", "DinnerId");

            rb.Property(reservation => reservation.Id)
                .HasColumnName("ReservationId")
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => ReservationId.Create(value));

            rb.Property(reservation => reservation.GuestId)
                .HasConversion(id => id.Value, value => GuestId.Create(value));
            rb.Property(reservation => reservation.BillId)
                .HasConversion(id => id.Value, value => BillId.Create(value));
        });
        builder.Metadata.FindNavigation(nameof(Dinner.Reservations))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
