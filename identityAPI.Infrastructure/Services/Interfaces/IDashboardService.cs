using identityAPI.Core.Models;

namespace identityAPI.Infrastructure.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<DashboardMetricsDto> GetMetricsAsync();
        Task<DashboardRecentDto> GetRecentAsync();
        Task<IEnumerable<DashboardNotificationDto>> GetNotificationsAsync();
    }
}
