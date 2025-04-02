using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasMany(u => u.Listings)
            .WithOne(l => l.Seller)
            .HasForeignKey(l => l.SellerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Bids)
            .WithOne(b => b.Bidder)
            .HasForeignKey(b => b.BidderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Reviews)
            .WithOne(r => r.Reviewer)
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.ReceivedReviews)
            .WithOne(r => r.Reviewee)
            .HasForeignKey(r => r.RevieweeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<Wallet>(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the many-to-many relationship with Auction through AuctionWatch
        builder.HasMany(u => u.AuctionWatches)
            .WithOne(aw => aw.User)
            .HasForeignKey(aw => aw.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship with won auctions
        builder.HasMany(u => u.WonAuctions)
            .WithOne(a => a.Winner)
            .HasForeignKey(a => a.WinnerId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}