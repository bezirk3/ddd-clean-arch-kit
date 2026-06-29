using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Guest;
using Forkfully.Domain.Guest.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forkfully.Infrastructure.Persistence.Configurations;

public class GuestConfigurations : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("Guests");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => GuestId.Create(value));

        builder.Property(g => g.UserId)
            .HasConversion(id => id.Value, value => UserId.Create(value));

        builder.Property(g => g.FirstName).HasMaxLength(100);
        builder.Property(g => g.LastName).HasMaxLength(100);

        builder.OwnsOne(g => g.AverageRating);

        builder.OwnsMany(g => g.UpcomingDinnerIds, b =>
        {
            b.ToTable("GuestUpcomingDinnerIds");
            b.WithOwner().HasForeignKey("GuestId");
            b.HasKey("Id");
            b.Property(dinnerId => dinnerId.Value).HasColumnName("DinnerId").ValueGeneratedNever();
        });
        builder.Metadata.FindNavigation(nameof(Guest.UpcomingDinnerIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(g => g.PastDinnerIds, b =>
        {
            b.ToTable("GuestPastDinnerIds");
            b.WithOwner().HasForeignKey("GuestId");
            b.HasKey("Id");
            b.Property(dinnerId => dinnerId.Value).HasColumnName("DinnerId").ValueGeneratedNever();
        });
        builder.Metadata.FindNavigation(nameof(Guest.PastDinnerIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(g => g.BillIds, b =>
        {
            b.ToTable("GuestBillIds");
            b.WithOwner().HasForeignKey("GuestId");
            b.HasKey("Id");
            b.Property(billId => billId.Value).HasColumnName("BillId").ValueGeneratedNever();
        });
        builder.Metadata.FindNavigation(nameof(Guest.BillIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(g => g.MenuReviewIds, b =>
        {
            b.ToTable("GuestMenuReviewIds");
            b.WithOwner().HasForeignKey("GuestId");
            b.HasKey("Id");
            b.Property(reviewId => reviewId.Value).HasColumnName("MenuReviewId").ValueGeneratedNever();
        });
        builder.Metadata.FindNavigation(nameof(Guest.MenuReviewIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(g => g.Ratings, rb =>
        {
            rb.ToTable("GuestRatings");
            rb.WithOwner().HasForeignKey("GuestId");
            rb.HasKey("Id", "GuestId");

            rb.Property(rating => rating.Id)
                .HasColumnName("GuestRatingId")
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => GuestRatingId.Create(value));

            rb.Property(rating => rating.HostId)
                .HasConversion(id => id.Value, value => HostId.Create(value));
            rb.Property(rating => rating.DinnerId)
                .HasConversion(id => id.Value, value => DinnerId.Create(value));
        });
        builder.Metadata.FindNavigation(nameof(Guest.Ratings))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
