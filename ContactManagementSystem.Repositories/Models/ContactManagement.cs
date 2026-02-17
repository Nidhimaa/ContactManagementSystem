namespace ContactManagementSystem.Repositories.Models
{
    public class ContactManagement
    {
        // Using SQL server - auto increment
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string MobileNo { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
    }
}