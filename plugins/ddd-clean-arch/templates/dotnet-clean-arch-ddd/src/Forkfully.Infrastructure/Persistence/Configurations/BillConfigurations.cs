using Forkfully.Domain.Bill;
using Forkfully.Domain.Bill.ValueObjects;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forkfully.Infrastructure.Persistence.Configurations;

public class BillConfigurations : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bills");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => BillId.Create(value));

        builder.Property(b => b.DinnerId)
            .HasConversion(id => id.Value, value => DinnerId.Create(value));

        builder.Property(b => b.GuestId)
            .HasConversion(id => id.Value, value => GuestId.Create(value));

        // Price value object → table-split into the Bills table.
        builder.OwnsOne(b => b.Amount, priceBuilder =>
            priceBuilder.Property(price => price.Amount).HasPrecision(18, 2));
    }
}
