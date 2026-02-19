using ContactManagementSystem.DTOs;

namespace ContactManagementSystem.Services.Interface
{
    public interface IAuditService
    {
        Task LogAuditEventAsync(AuditEventDTO logEvent);
    }
}