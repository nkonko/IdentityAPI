using identityAPI.Core.Models;
using identityAPI.Infrastructure.Persistence;
using identityAPI.Infrastructure.Services.Interfaces;

namespace identityAPI.Infrastructure.Services
{
    

    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<AuditLogDto>> GetLogsAsync()
        {
            return Task.FromResult<IEnumerable<AuditLogDto>>(new List<AuditLogDto>());
        }

        public Task<IEnumerable<AuditLogDto>> GetLogsByUserAsync(string userId)
        {
            return Task.FromResult<IEnumerable<AuditLogDto>>(new List<AuditLogDto>());
        }
    }
}
