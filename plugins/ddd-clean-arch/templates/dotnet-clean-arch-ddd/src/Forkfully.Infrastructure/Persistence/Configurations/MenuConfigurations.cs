using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu;
using Forkfully.Domain.Menu.Entities;
using Forkfully.Domain.Menu.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forkfully.Infrastructure.Persistence.Configurations;

public class MenuConfigurations : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        ConfigureMenusTable(builder);
        ConfigureMenuSectionsTable(builder);
        ConfigureMenuDinnerIdsTable(builder);
        ConfigureMenuReviewIdsTable(builder);
    }

    private void ConfigureMenusTable(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");

        // The aggregate id is the primary key (unique within the entire system).
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever()          // ids are generated in the domain, not the DB
            .HasConversion(
                id => id.Value,
                value => MenuId.Create(value));

        builder.Property(m => m.Name).HasMaxLength(100);
        builder.Property(m => m.Description).HasMaxLength(100);

        // AverageRating is a value object → table-split into the Menus table.
        builder.OwnsOne(m => m.AverageRating);

        // Flatten the HostId value object to its underlying value.
        builder.Property(m => m.HostId)
            .HasConversion(
                id => id.Value,
                value => HostId.Create(value));
    }

    private void ConfigureMenuSectionsTable(EntityTypeBuilder<Menu> builder)
    {
        builder.OwnsMany(m => m.Sections, sb =>
        {
            sb.ToTable("MenuSections");

            // Foreign key back to the owning menu.
            sb.WithOwner().HasForeignKey("MenuId");

            // Composite key: the section is unique *within* the menu aggregate.
            sb.HasKey("Id", "MenuId");

            sb.Property(s => s.Id)
                .HasColumnName("MenuSectionId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => MenuSectionId.Create(value));

            sb.Property(s => s.Name).HasMaxLength(100);
            sb.Property(s => s.Description).HasMaxLength(100);

            sb.OwnsMany(s => s.Items, ib =>
            {
                ib.ToTable("MenuItems");

                // Two foreign keys: the item is reached via section *and* menu.
                ib.WithOwner().HasForeignKey("MenuSectionId", "MenuId");

                // Composite key — unique within the entire system.
                ib.HasKey("Id", "MenuSectionId", "MenuId");

                ib.Property(i => i.Id)
                    .HasColumnName("MenuItemId")
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => MenuItemId.Create(value));

                ib.Property(i => i.Name).HasMaxLength(100);
                ib.Property(i => i.Description).HasMaxLength(100);
            });

            // EF must populate the private backing field, not the read-only property.
            sb.Navigation(s => s.Items).Metadata.SetField("_items");
            sb.Navigation(s => s.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Metadata.FindNavigation(nameof(Menu.Sections))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureMenuDinnerIdsTable(EntityTypeBuilder<Menu> builder)
    {
        // A list of value objects → its own table with a surrogate auto-increment key,
        // so we faithfully reconstruct the list regardless of the value-object values.
        builder.OwnsMany(m => m.DinnerIds, db =>
        {
            db.ToTable("MenuDinnerIds");

            db.WithOwner().HasForeignKey("MenuId");

            db.HasKey("Id");

            db.Property(d => d.Value)
                .HasColumnName("DinnerId")
                .ValueGeneratedNever();
        });

        builder.Metadata.FindNavigation(nameof(Menu.DinnerIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureMenuReviewIdsTable(EntityTypeBuilder<Menu> builder)
    {
        builder.OwnsMany(m => m.MenuReviewIds, rb =>
        {
            rb.ToTable("MenuReviewIds");

            rb.WithOwner().HasForeignKey("MenuId");

            rb.HasKey("Id");

            rb.Property(r => r.Value)
                .HasColumnName("MenuReviewId")
                .ValueGeneratedNever();
        });

        builder.Metadata.FindNavigation(nameof(Menu.MenuReviewIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
