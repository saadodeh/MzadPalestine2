namespace MzadPalestine.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IAuctionRepository Auctions { get; }
    IBidRepository Bids { get; }
    IListingRepository Listings { get; }
    ICategoryRepository Categories { get; }
    ILocationRepository Locations { get; }
    ITagRepository Tags { get; }
    IReviewRepository Reviews { get; }
    IMessageRepository Messages { get; }
    INotificationRepository Notifications { get; }
    IPaymentRepository Payments { get; }
    IInvoiceRepository Invoices { get; }
    IDisputeRepository Disputes { get; }
    IWatchlistRepository Watchlists { get; }
    ISubscriptionRepository Subscriptions { get; }
    IReportRepository Reports { get; }
    Task<bool> Complete();
    bool HasChanges();
}
