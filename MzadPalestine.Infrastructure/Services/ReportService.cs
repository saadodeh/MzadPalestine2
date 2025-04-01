using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.DTOs.Reports;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object> CreateReportAsync(CreateReportDto reportDto, string userId)
    {
        var report = new Report
        {
            Title = reportDto.Title,
            Description = reportDto.Description,
            ReportedUserId = reportDto.ReportedUserId,
            ReporterId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();

        return report;
    }

    public async Task<object> GetUserReportsAsync(string userId)
    {
        return await _context.Reports
            .Where(r => r.ReporterId == userId || r.ReportedUserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<object> GetReportByIdAsync(string reportId)
    {
        return await _context.Reports
            .FirstOrDefaultAsync(r => r.Id == int.Parse(reportId));
    }

    public async Task<object> UpdateReportStatusAsync(UpdateReportStatusDto updateDto, string userId)
    {
        var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == int.Parse(updateDto.ReportId));
        if (report == null)
            throw new Exception("Report not found");

        if (Enum.TryParse<ReportStatus>(updateDto.Status, out var status))
        {
            report.Status = status;
            report.ResolvedById = userId;
            report.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return report;
        }

        throw new Exception("Invalid report status");
    }

    public async Task<object> AddReportCommentAsync(AddReportCommentDto commentDto, string userId)
    {
        var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == int.Parse(commentDto.ReportId));
        if (report == null)
            throw new Exception("Report not found");

        var comment = new ReportComment
        {
            Content = commentDto.Comment,
            UserId = userId,
            ReportId = int.Parse(commentDto.ReportId),
            CreatedAt = DateTime.UtcNow
        };

        await _context.ReportComments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return comment;
    }
}