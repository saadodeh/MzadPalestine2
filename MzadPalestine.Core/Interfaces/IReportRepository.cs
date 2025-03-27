using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;  //  √ﬂœ „‰ «” Ì—«œ Enums

namespace MzadPalestine.Core.Interfaces;

public interface IReportRepository : IGenericRepository<Report>
{
    Task<Report?> GetReportWithDetailsAsync(int reportId);
    Task<PagedList<Report>> GetUserReportsAsync(string userId, PaginationParams parameters);
    Task<PagedList<Report>> GetReportedUserReportsAsync(string reportedUserId, PaginationParams parameters);
    Task<PagedList<Report>> GetReportsByTypeAsync(MzadPalestine.Core.Models.Enums.ReportType type, PaginationParams parameters); // «” Œœ„ «·„”«— «·ﬂ«„· ·‹ ReportType
    Task<bool> HasActiveReportAsync(string userId, string reportedUserId);
    Task UpdateReportStatusAsync(int reportId, MzadPalestine.Core.Models.Enums.ReportStatus status); // «” Œœ„ «·„”«— «·ﬂ«„· ·‹ ReportStatus
    Task AddReportCommentAsync(int reportId, ReportComment comment);
    Task<IEnumerable<ReportComment>> GetReportCommentsAsync(int reportId);
    Task<bool> IsReportCreatorAsync(string userId, int reportId);
    Task<Dictionary<MzadPalestine.Core.Models.Enums.ReportType, int>> GetReportStatisticsAsync(); // «” Œœ„ «·„”«— «·ﬂ«„· ·‹ ReportType
}
