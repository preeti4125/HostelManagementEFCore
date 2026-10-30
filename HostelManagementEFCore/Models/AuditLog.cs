namespace HostelManagementEFCore.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string Action { get; set; } = string.Empty;       // e.g., "CREATE", "UPDATE", "DELETE"
        public string EntityName { get; set; } = string.Empty;   // e.g., "Room", "Student"
        public string Details { get; set; } = string.Empty;      // Description of the operation
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}