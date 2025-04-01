namespace MzadPalestine.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAuctionRepository Auctions { get; }
    IDisputeRepository Disputes { get; }
    IDisputeEvidenceRepository DisputeEvidences { get; }
    IBidRepository Bids { get; }
    IAutoBidRepository AutoBids { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
