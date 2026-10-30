using HostelManagementEFCore.Models;

namespace HostelManagementEFCore.Repositories
{
    public interface IAuditLogRepository
    {
        Task LogAsync(string action, string entityName, string details);
        Task<List<AuditLog>> GetAllLogsAsync();
        Task SaveChangesAsync();
    }
}