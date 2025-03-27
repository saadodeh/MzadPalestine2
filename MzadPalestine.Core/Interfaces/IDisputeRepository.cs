using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IDisputeRepository : IGenericRepository<Dispute>
{
    Task<Dispute?> GetDisputeWithDetailsAsync(int disputeId);
    Task<PagedList<Dispute>> GetUserDisputesAsync(string userId, PaginationParams parameters);
    Task<PagedList<Dispute>> GetAuctionDisputesAsync(int auctionId, PaginationParams parameters);
    Task<bool> HasOpenDisputeAsync(string userId, int auctionId);
    Task<IEnumerable<Dispute>> GetOpenDisputesAsync();
    Task UpdateDisputeStatusAsync(int disputeId, DisputeStatus status);
    Task AddDisputeEvidenceAsync(int disputeId, DisputeEvidence evidence);
    Task<IEnumerable<DisputeEvidence>> GetDisputeEvidenceAsync(int disputeId);
    Task<bool> IsDisputeParticipantAsync(string userId, int disputeId);
    Task<Dictionary<DisputeStatus, int>> GetDisputeStatisticsAsync(string userId);
}
