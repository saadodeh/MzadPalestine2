using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Interfaces.Common;
using MzadPalestine.Core.Models.Common;
using System.Reflection;
using System.Linq.Expressions;

namespace MzadPalestine.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<Listing> Listings { get; set; } = null!;
    public DbSet<Auction> Auctions { get; set; } = null!;
    public DbSet<Bid> Bids { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<Report> Reports { get; set; } = null!;
    public DbSet<ReportComment> ReportComments { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<ListingImage> ListingImages { get; set; } = null!;
    public DbSet<AuctionWatch> AuctionWatches { get; set; } = null!;
    public DbSet<AutoBid> AutoBids { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Wallet> Wallets { get; set; } = null!;
    public DbSet<WalletTransaction> WalletTransactions { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<Dispute> Disputes { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<Watchlist> Watchlists { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<CustomerSupportTicket> CustomerSupportTickets { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply all configurations from the current assembly
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Add any additional model configurations here
        builder.HasDefaultSchema("mzad");

        // Configure many-to-many relationship between Listings and Tags
        builder.Entity<Listing>()
            .HasMany(l => l.Tags)
            .WithMany(t => t.Listings)
            .UsingEntity(j => j.ToTable("ListingTags"));

        // Configure many-to-many relationship between Auction and ApplicationUser through AuctionWatch
        builder.Entity<AuctionWatch>()
            .ToTable("AuctionWatchers")
            .HasKey(aw => new { aw.UserId, aw.AuctionId });

        // Configure Listing-User relationship
        builder.Entity<Listing>()
            .HasOne(l => l.Seller)
            .WithMany(u => u.Listings)
            .HasForeignKey(l => l.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Auction relationships
        builder.Entity<Auction>()
            .HasOne(a => a.Winner)
            .WithMany(u => u.WonAuctions)
            .HasForeignKey(a => a.WinnerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Auction>()
            .HasOne(a => a.Listing)
            .WithOne()
            .HasForeignKey<Auction>(a => a.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Review relationships
        builder.Entity<Review>()
            .HasOne(r => r.Reviewer)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Review>()
            .HasOne(r => r.Reviewee)
            .WithMany(u => u.ReceivedReviews)
            .HasForeignKey(r => r.RevieweeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Message relationships
        builder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Report relationships
        builder.Entity<Report>()
            .HasOne(r => r.ResolvedBy)
            .WithMany()
            .HasForeignKey(r => r.ResolvedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Dispute relationships
        builder.Entity<Dispute>()
            .HasOne(d => d.ResolvedBy)
            .WithMany()
            .HasForeignKey(d => d.ResolvedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Invoice relationships
        builder.Entity<Invoice>()
            .HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure WalletTransaction relationships
        builder.Entity<WalletTransaction>()
            .HasOne(wt => wt.User)
            .WithMany()
            .HasForeignKey(wt => wt.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Invoice>()
            .HasOne(i => i.Auction)
            .WithMany()
            .HasForeignKey(i => i.AuctionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Bid relationships
        builder.Entity<Bid>()
            .HasOne(b => b.Auction)
            .WithMany(a => a.Bids)
            .HasForeignKey(b => b.AuctionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Bid>()
            .HasOne(b => b.Bidder)
            .WithMany(u => u.Bids)
            .HasForeignKey(b => b.BidderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure AutoBid relationships
        builder.Entity<AutoBid>()
            .HasOne(ab => ab.Auction)
            .WithMany(a => a.AutoBids)
            .HasForeignKey(ab => ab.AuctionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AutoBid>()
            .HasOne(ab => ab.Bidder)
            .WithMany()
            .HasForeignKey(ab => ab.BidderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure indexes
        builder.Entity<Listing>()
            .HasIndex(l => l.Status);

        builder.Entity<Auction>()
            .HasIndex(a => a.Status);

        builder.Entity<Payment>()
            .HasIndex(p => p.Status);

        builder.Entity<Notification>()
            .HasIndex(n => n.Status);

        builder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();

        builder.Entity<Watchlist>()
            .HasIndex(w => new { w.UserId , w.ListingId })
            .IsUnique();

        // Configure soft delete filter
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                entityType.GetProperty("IsDeleted")?.SetDefaultValue(false);
                var parameter = Expression.Parameter(entityType.ClrType , "e");
                var body = Expression.Equal(
                    Expression.Property(parameter , "IsDeleted") ,
                    Expression.Constant(false)
                );
                var lambda = Expression.Lambda(body , parameter);
                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntities( )
    {
        var entries = ChangeTracker.Entries<IAuditable>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = GetCurrentUserId();
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedBy = GetCurrentUserId();
            }
        }
    }

    private string? GetCurrentUserId( )
    {
        // This should be implemented to get the current user ID from the ICurrentUserService
        // For now, return null or implement your logic
        return null;
    }
}
