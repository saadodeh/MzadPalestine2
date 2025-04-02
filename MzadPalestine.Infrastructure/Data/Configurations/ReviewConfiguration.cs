using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.Property(r => r.Rating)
            .IsRequired()
            .HasColumnType("decimal(3,1)");

        builder.Property(r => r.Comment)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // Indexes
        builder.HasIndex(r => r.ReviewerId);
        builder.HasIndex(r => r.Rating);
        builder.HasIndex(r => r.CreatedAt);

        // Relationships
        builder.HasOne(r => r.Reviewer)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Reviewee)
            .WithMany(u => u.ReceivedReviews)
            .HasForeignKey(r => r.RevieweeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Optional relationship with an auction
        builder.HasOne(r => r.Auction)
            .WithMany(a => a.Reviews) // �� ����� ������� ���
            .HasForeignKey(r => r.AuctionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ensure a user can only review another user once per auction
        builder.HasIndex(r => new { r.ReviewerId, r.AuctionId }) // �� ����� ������
            .IsUnique()
            .HasFilter("[AuctionId] IS NOT NULL");
    }
}
