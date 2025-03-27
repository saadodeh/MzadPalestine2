using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Infrastructure.Data.Configurations;

public class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.ToTable("Listings");

        builder.Property(l => l.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(l => l.Description)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(l => l.StartingPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(l => l.ReservePrice)
            .HasPrecision(18, 2);

        builder.Property(l => l.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(l => l.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // Indexes
        builder.HasIndex(l => l.Status);
        builder.HasIndex(l => l.CreatedAt);
        builder.HasIndex(l => new { l.CategoryId, l.Status });
        builder.HasIndex(l => new { l.LocationId, l.Status });

        // Relationships
        builder.HasOne(l => l.Category)
            .WithMany(c => c.Listings)
            .HasForeignKey(l => l.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Location)
            .WithMany(loc => loc.Listings)
            .HasForeignKey(l => l.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Seller)
            .WithMany(u => u.Listings)
            .HasForeignKey(l => l.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Auction)
            .WithOne(a => a.Listing)
            .HasForeignKey<Auction>(a => a.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(l => l.Images)
            .WithOne(i => i.Listing)
            .HasForeignKey(i => i.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(l => l.Reports)
            .WithOne(r => r.Listing)
            .HasForeignKey(r => r.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
