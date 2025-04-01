using MzadPalestine.Core.DTOs.Disputes;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IDisputeService
{
    Task<Result<DisputeDto>> CreateDisputeAsync(string userId, CreateDisputeDto disputeDto);
    Task<Result<DisputeDto>> GetDisputeByIdAsync(string userId, int disputeId);
    Task<PagedResult<DisputeDto>> GetUserDisputesAsync(string userId, PaginationParams parameters);
    Task<Result<DisputeDto>> ResolveDisputeAsync(string adminId, int disputeId, ResolveDisputeDto resolveDto);
    Task<Result<DisputeEvidenceDto>> AddDisputeEvidenceAsync(string userId, int disputeId, AddDisputeEvidenceRequestDto evidenceDto);
    Task<PagedResult<DisputeDto>> GetAuctionDisputesAsync(int auctionId, PaginationParams parameters);
}