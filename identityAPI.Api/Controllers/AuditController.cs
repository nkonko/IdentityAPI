using identityAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using identityAPI.Infrastructure.Services;

namespace identityAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet("logs")]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetLogs()
            => Ok(await _auditService.GetLogsAsync());

        [HttpGet("logs/{userId}")]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetLogsByUser(string userId)
            => Ok(await _auditService.GetLogsByUserAsync(userId));
    }
}
