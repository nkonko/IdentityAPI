using identityAPI.Core.Models;
using identityAPI.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace identityAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
            => Ok(await _dashboardService.GetSummaryAsync());

        [HttpGet("metrics")]
        public async Task<ActionResult<DashboardMetricsDto>> GetMetrics()
            => Ok(await _dashboardService.GetMetricsAsync());

        [HttpGet("recent")]
        public async Task<ActionResult<DashboardRecentDto>> GetRecent()
            => Ok(await _dashboardService.GetRecentAsync());

        [HttpGet("notifications")]
        public async Task<ActionResult<IEnumerable<DashboardNotificationDto>>> GetNotifications()
            => Ok(await _dashboardService.GetNotificationsAsync());
    }
}
