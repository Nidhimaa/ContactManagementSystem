using ContactManagementSystem.DTOs;
using ContactManagementSystem.Repositories;
using ContactManagementSystem.Repositories.Models;
using ContactManagementSystem.Services.Interface;

namespace ContactManagementSystem.Services.Service
{
    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;

        public AuditService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAuditEventAsync(AuditEventDTO logEvent)
        {
            var indiaTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, 
                TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            var audit = new AuditEvent
            {
                UserAction = logEvent.UserAction,
                UserEmail = logEvent.UserEmail,
                UserActionTime = indiaTime
            };

            _context.AuditEvents.Add(audit);
            await _context.SaveChangesAsync();
        }
    }
}