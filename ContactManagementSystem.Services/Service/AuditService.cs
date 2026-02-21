using ContactManagementSystem.DTOs;
using ContactManagementSystem.Repositories;
using ContactManagementSystem.Repositories.Models;
using ContactManagementSystem.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace ContactManagementSystem.Services.Service
{
    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAuditEventAsync(AuditEventDTO logEvent)
        {
            var indiaTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            var endpoint = _httpContextAccessor.HttpContext?
                .Request.Path.Value ?? "unknown";
            var method = _httpContextAccessor.HttpContext?
                .Request.Method ?? "unknown";

            var audit = new AuditEvent
            {
                UserAction = logEvent.UserAction,
                UserEmail = logEvent.UserEmail,
                ContactId = logEvent.ContactId,
                Endpoint = $"{method} {endpoint}",
                UserActionTime = indiaTime
            };

            _context.AuditEvents.Add(audit);
            await _context.SaveChangesAsync();
        }
    }
}