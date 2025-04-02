using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Infrastructure.Data.Configurations;

public class ReportCommentConfiguration : IEntityTypeConfiguration<ReportComment>
{
    public void Configure(EntityTypeBuilder<ReportComment> builder)
    {
        builder.HasKey(rc => rc.Id);

        builder.Property(rc => rc.Content)
            .IsRequired();

        builder.Property(rc => rc.CreatedAt)
            .IsRequired();

        builder.Property(rc => rc.UserId)
            .IsRequired();

        builder.HasOne(rc => rc.User)
            .WithMany()
            .HasForeignKey(rc => rc.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(rc => rc.Report)
            .WithMany()
            .HasForeignKey(rc => rc.ReportId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}