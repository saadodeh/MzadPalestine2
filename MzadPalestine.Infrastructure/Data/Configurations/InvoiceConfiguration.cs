using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Infrastructure.Data.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.DueDate)
            .IsRequired();

        builder.Property(i => i.Status)
            .IsRequired();

        builder.Property(i => i.IssuedAt)
            .IsRequired();

        builder.Property(i => i.UserId)
            .IsRequired();

        builder.HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(i => i.Auction)
            .WithMany(a => a.Invoices)
            .HasForeignKey(i => i.AuctionId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}