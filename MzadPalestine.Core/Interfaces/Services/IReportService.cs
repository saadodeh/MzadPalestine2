using MzadPalestine.Core.DTOs.Reports;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IReportService
{
    Task<object> CreateReportAsync(CreateReportDto reportDto, string userId);
    Task<object> GetUserReportsAsync(string userId);
    Task<object> GetReportByIdAsync(string reportId);
    Task<object> UpdateReportStatusAsync(UpdateReportStatusDto updateDto, string userId);
    Task<object> AddReportCommentAsync(AddReportCommentDto commentDto, string userId);
}