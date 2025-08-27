using identityAPI.Core.Models;
using identityAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace identityAPI.Infrastructure.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<DashboardMetricsDto> GetMetricsAsync();
        Task<DashboardRecentDto> GetRecentAsync();
        Task<IEnumerable<DashboardNotificationDto>> GetNotificationsAsync();
    }

    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            return new DashboardSummaryDto
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalRoles = await _context.Roles.CountAsync(),
                TotalSubscriptions = 0 // Placeholder
            };
        }

        public async Task<DashboardMetricsDto> GetMetricsAsync()
        {
            return new DashboardMetricsDto
            {
                ActiveUsers = await _context.Users.CountAsync(), // Placeholder
                NewUsersThisMonth = 0, // Placeholder
                RevenueThisMonth = 0 // Placeholder
            };
        }

        public async Task<DashboardRecentDto> GetRecentAsync()
        {
            return new DashboardRecentDto
            {
                RecentActivities = new List<string>() // Placeholder
            };
        }

        public async Task<IEnumerable<DashboardNotificationDto>> GetNotificationsAsync()
        {
            return new List<DashboardNotificationDto>(); // Placeholder
        }
    }
}
