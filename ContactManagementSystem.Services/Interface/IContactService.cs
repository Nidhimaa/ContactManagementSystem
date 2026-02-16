using ContactManagementSystem.DTOs;

namespace ContactManagementSystem.Services.Interface
{
    public interface IContactService
    {
        Task<ContactResponseDTO> AddContactAsync(ContactRequestDTO request);

        Task<IEnumerable<ContactResponseDTO>> GetAllContactsAsync();

        Task<ContactResponseDTO?> GetContactByEmailAsync(string email);

        Task<ContactResponseDTO> UpdateContactAsync(
            string email,
            UpdateContactDTO updateRequest);

        Task<bool> DeleteContactAsync(string email);
    }
}