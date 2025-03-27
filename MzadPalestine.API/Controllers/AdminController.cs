using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Admin;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IAuctionService _auctionService;
    private readonly ICurrentUserService _currentUserService;

    public AdminController(
        IAdminService adminService,
        IAuctionService auctionService,
        ICurrentUserService currentUserService)
    {
        _adminService = adminService;
        _auctionService = auctionService;
        _currentUserService = currentUserService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] PaginationParams parameters)
    {
        var result = await _adminService.GetAllUsersAsync(parameters);
        return Ok(result);
    }

    [HttpPut("ban-user/{userId}")]
    public async Task<IActionResult> BanUser(string userId, [FromBody] BanUserDto banUserDto)
    {
        var adminId = _currentUserService.GetUserId();
        var result = await _adminService.BanUserAsync(adminId, userId, banUserDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "User banned successfully" });
    }

    [HttpPut("unban-user/{userId}")]
    public async Task<IActionResult> UnbanUser(string userId)
    {
        var adminId = _currentUserService.GetUserId();
        var result = await _adminService.UnbanUserAsync(adminId, userId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "User unbanned successfully" });
    }

    [HttpDelete("delete-auction/{auctionId}")]
    public async Task<IActionResult> DeleteAuction(int auctionId, [FromBody] DeleteAuctionDto deleteAuctionDto)
    {
        var adminId = _currentUserService.GetUserId();
        var result = await _adminService.DeleteAuctionAsync(adminId, auctionId, deleteAuctionDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Auction deleted successfully" });
    }

    [HttpGet("reports")]
    public async Task<IActionResult> GetAllReports([FromQuery] PaginationParams parameters)
    {
        var result = await _adminService.GetAllReportsAsync(parameters);
        return Ok(result);
    }

    [HttpPut("resolve-report/{reportId}")]
    public async Task<IActionResult> ResolveReport(int reportId, [FromBody] ResolveReportDto resolveReportDto)
    {
        var adminId = _currentUserService.GetUserId();
        var result = await _adminService.ResolveReportAsync(adminId, reportId, resolveReportDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Report resolved successfully" });
    }

    [HttpGet("dashboard/stats")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var result = await _adminService.GetDashboardStatsAsync();
        return Ok(result);
    }

    [HttpGet("audit-logs")]
    public async Task<IActionResult> GetAuditLogs([FromQuery] PaginationParams parameters)
    {
        var result = await _adminService.GetAuditLogsAsync(parameters);
        return Ok(result);
    }
}
