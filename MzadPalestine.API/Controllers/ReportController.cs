using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Reports;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ICurrentUserService _currentUserService;

    public ReportController(IReportService reportService , ICurrentUserService currentUserService)
    {
        _reportService = reportService;
        _currentUserService = currentUserService;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitReport([FromBody] CreateReportDto reportDto)
    {
        var result = await _reportService.CreateReportAsync(reportDto , _currentUserService.UserId!);
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserReports(string userId)
    {
        if (_currentUserService.UserId != userId && !_currentUserService.IsAdmin)
            return Forbid();

        var result = await _reportService.GetUserReportsAsync(userId);
        return Ok(result);
    }

    [HttpGet("{reportId}")]
    public async Task<IActionResult> GetReport(string reportId)
    {
        var result = await _reportService.GetReportByIdAsync(reportId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("update/{reportId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateReportStatus([FromBody] UpdateReportStatusDto updateDto)
    {
        var result = await _reportService.UpdateReportStatusAsync(updateDto , _currentUserService.UserId!);
        return Ok(result);
    }

    [HttpPost("comment")]
    public async Task<IActionResult> AddReportComment([FromBody] AddReportCommentDto commentDto)
    {
        var result = await _reportService.AddReportCommentAsync(commentDto , _currentUserService.UserId!);
        return Ok(result);
    }
}
