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

    public ReportController(IReportService reportService, ICurrentUserService currentUserService)
    {
        _reportService = reportService;
        _currentUserService = currentUserService;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitReport([FromBody] CreateReportDto reportDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _reportService.CreateReportAsync(userId, reportDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return CreatedAtAction(nameof(GetReport), new { reportId = result.Data.Id }, result.Data);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserReports(string userId, [FromQuery] PaginationParams parameters)
    {
        var currentUserId = _currentUserService.GetUserId();
        if (currentUserId != userId && !User.IsInRole("Admin"))
            return Forbid();

        var result = await _reportService.GetUserReportsAsync(userId, parameters);
        return Ok(result);
    }

    [HttpGet("{reportId}")]
    public async Task<IActionResult> GetReport(int reportId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _reportService.GetReportByIdAsync(userId, reportId);
        
        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    [HttpPut("update/{reportId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateReportStatus(int reportId, [FromBody] UpdateReportStatusDto updateDto)
    {
        var adminId = _currentUserService.GetUserId();
        var result = await _reportService.UpdateReportStatusAsync(adminId, reportId, updateDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpPost("comment/{reportId}")]
    public async Task<IActionResult> AddReportComment(int reportId, [FromBody] AddReportCommentDto commentDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _reportService.AddReportCommentAsync(userId, reportId, commentDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }
}
