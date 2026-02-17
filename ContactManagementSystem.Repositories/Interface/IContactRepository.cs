using ContactManagementSystem.Repositories.Models;

namespace ContactManagementSystem.Repositories.Interface
{
    public interface IContactRepository
    {
        Task<List<ContactManagement>> GetAllContactsAsync();

        Task<ContactManagement?> GetByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email);
    }
}