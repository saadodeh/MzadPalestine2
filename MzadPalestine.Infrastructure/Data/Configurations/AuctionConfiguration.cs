using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Infrastructure.Data.Configurations;

public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.StartDate)
            .IsRequired();

        builder.Property(a => a.EndDate)
            .IsRequired();

        builder.Property(a => a.CurrentPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(a => a.MinimumBidIncrement)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(a => a.Status)
            .IsRequired();

        builder.HasOne(a => a.Listing)
            .WithOne()
            .HasForeignKey<Auction>(a => a.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Winner)
            .WithMany()
            .HasForeignKey(a => a.WinnerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(a => a.Watchers)
            .WithMany()
            .UsingEntity(j => j.ToTable("AuctionWatchers"));

        builder.HasMany(a => a.AutoBids)
            .WithOne(ab => ab.Auction)
            .HasForeignKey(ab => ab.AuctionId)
            .OnDelete(DeleteBehavior.Cascade);

        // التحقق من أن EndDate يأتي بعد StartDate
        builder.HasCheckConstraint("CK_Auction_EndDate_After_StartDate", 
            "[EndDate] > [StartDate]");
    }
}
