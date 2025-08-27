namespace identityAPI.Core.Models
{
    public class DashboardSummaryDto
    {
        public int TotalUsers { get; set; }
        public int TotalRoles { get; set; }
        public int TotalSubscriptions { get; set; }
    }

    public class DashboardMetricsDto
    {
        public int ActiveUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int RevenueThisMonth { get; set; }
    }

    public class DashboardRecentDto
    {
        public IList<string> RecentActivities { get; set; } = new List<string>();
    }

    public class DashboardNotificationDto
    {
        public string Message { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
