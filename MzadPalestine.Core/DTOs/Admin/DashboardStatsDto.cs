using System;

namespace MzadPalestine.Core.DTOs.Admin;

public class DashboardStatsDto
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int BannedUsers { get; set; }
    public int SuspendedUsers { get; set; }
    public int TotalListings { get; set; }
    public int ActiveListings { get; set; }
    public int TotalAuctions { get; set; }
    public int ActiveAuctions { get; set; }
    public int TotalReports { get; set; }
    public int PendingReports { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public int NewUsersToday { get; set; }
    public int NewListingsToday { get; set; }
    public DateTime LastUpdated { get; set; }
}