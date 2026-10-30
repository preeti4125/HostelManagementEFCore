using Microsoft.EntityFrameworkCore;
using HostelManagementEFCore.Data;
using HostelManagementEFCore.Models;

namespace HostelManagementEFCore.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly HostelDbContext _context;

        public AuditLogRepository(HostelDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string action, string entityName, string details)
        {
            var log = new AuditLog
            {
                Action = action,
                EntityName = entityName,
                Details = details,
                Timestamp = DateTime.UtcNow
            };

            await _context.AuditLogs.AddAsync(log);
        }

        public async Task<List<AuditLog>> GetAllLogsAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}