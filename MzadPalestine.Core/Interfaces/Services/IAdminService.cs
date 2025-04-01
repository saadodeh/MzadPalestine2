using MzadPalestine.Core.DTOs.Admin;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IAdminService
{
    Task<PagedList<UserDto>> GetAllUsersAsync(PaginationParams parameters);
    Task<Result<bool>> BanUserAsync(string adminId, string userId, BanUserDto banUserDto);
    Task<Result<bool>> UnbanUserAsync(string adminId, string userId);
    Task<Result<bool>> DeleteAuctionAsync(string adminId, int auctionId, DeleteAuctionDto deleteAuctionDto);
    Task<PagedList<ReportDto>> GetAllReportsAsync(PaginationParams parameters);
    Task<Result<bool>> ResolveReportAsync(string adminId, int reportId, ResolveReportDto resolveReportDto);
    Task<DashboardStatsDto> GetDashboardStatsAsync();
    Task<PagedList<AuditLogDto>> GetAuditLogsAsync(PaginationParams parameters);
    Task<Result<bool>> SuspendUserAsync(string userId, string reason, DateTime? suspensionEnd = null);
    Task<Result<bool>> UnsuspendUserAsync(string userId);
    Task<Result<bool>> DeleteListingAsync(int listingId, string reason, bool notifyUser = true);
    Task<Result<bool>> ToggleUserVerificationAsync(string userId, bool isVerified);
}