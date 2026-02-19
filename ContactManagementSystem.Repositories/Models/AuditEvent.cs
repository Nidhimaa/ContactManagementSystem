namespace ContactManagementSystem.Repositories.Models
{
    public class AuditEvent
    {
        public int Id { get; set; }

        public string UserAction { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public string AppName { get; set; } = "contact api";

        public DateTime UserActionTime { get; set; }
    }
}