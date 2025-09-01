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

        public async Task<IEnumerable<AuditLogDto>> GetLogsAsync()
        {
            return new List<AuditLogDto>();
        }

        public async Task<IEnumerable<AuditLogDto>> GetLogsByUserAsync(string userId)
        {
            return new List<AuditLogDto>();
        }
    }
}
