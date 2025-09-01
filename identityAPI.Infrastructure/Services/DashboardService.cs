using identityAPI.Core.Models;
using identityAPI.Infrastructure.Persistence;
using identityAPI.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace identityAPI.Infrastructure.Services
{
    
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
                TotalSubscriptions = 0 
            };
        }

        public async Task<DashboardMetricsDto> GetMetricsAsync()
        {
            return new DashboardMetricsDto
            {
                ActiveUsers = await _context.Users.CountAsync(), 
                NewUsersThisMonth = 0,
                RevenueThisMonth = 0 
            };
        }

        public async Task<DashboardRecentDto> GetRecentAsync()
        {
            return new DashboardRecentDto
            {
                RecentActivities = new List<string>() 
            };
        }

        public async Task<IEnumerable<DashboardNotificationDto>> GetNotificationsAsync()
        {
            return new List<DashboardNotificationDto>(); 
        }
    }
}
