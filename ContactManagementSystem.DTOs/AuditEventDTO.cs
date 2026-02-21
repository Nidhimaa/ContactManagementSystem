namespace ContactManagementSystem.DTOs
{
    public class AuditEventDTO
    {
        public string UserAction { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public string Endpoint { get; set; } = string.Empty;

        public Guid? ContactId { get; set; }
    }
}