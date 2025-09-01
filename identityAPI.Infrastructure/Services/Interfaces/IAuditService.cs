using identityAPI.Core.Models;

namespace identityAPI.Infrastructure.Services.Interfaces
{
    public interface IAuditService
    {
        Task<IEnumerable<AuditLogDto>> GetLogsAsync();
        Task<IEnumerable<AuditLogDto>> GetLogsByUserAsync(string userId);
    }
}
