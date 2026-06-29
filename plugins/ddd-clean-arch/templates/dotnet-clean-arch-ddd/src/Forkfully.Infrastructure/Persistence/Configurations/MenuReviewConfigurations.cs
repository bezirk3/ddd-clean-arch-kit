using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu.ValueObjects;
using Forkfully.Domain.MenuReview;
using Forkfully.Domain.MenuReview.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forkfully.Infrastructure.Persistence.Configurations;

public class MenuReviewConfigurations : IEntityTypeConfiguration<MenuReview>
{
    public void Configure(EntityTypeBuilder<MenuReview> builder)
    {
        builder.ToTable("MenuReviews");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => MenuReviewId.Create(value));

        builder.Property(r => r.Comment).HasMaxLength(1000);

        builder.Property(r => r.HostId)
            .HasConversion(id => id.Value, value => HostId.Create(value));
        builder.Property(r => r.MenuId)
            .HasConversion(id => id.Value, value => MenuId.Create(value));
        builder.Property(r => r.GuestId)
            .HasConversion(id => id.Value, value => GuestId.Create(value));
        builder.Property(r => r.DinnerId)
            .HasConversion(id => id.Value, value => DinnerId.Create(value));
    }
}
