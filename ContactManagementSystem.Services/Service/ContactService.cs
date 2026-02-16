using AutoMapper;
using ContactManagementSystem.DTOs;
using ContactManagementSystem.Repositories;
using ContactManagementSystem.Repositories.Interface;
using ContactManagementSystem.Repositories.Models;
using ContactManagementSystem.Services.Interface;

namespace ContactManagementSystem.Services.Service
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactService(
            IContactRepository repository,
            AppDbContext context,
            IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        // CREATE
        public async Task<ContactResponseDTO> AddContactAsync(
            ContactRequestDTO request)
        {
            var entity = _mapper.Map<ContactManagement>(request);

            entity.IsDeleted = false;

            _context.Contacts.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<ContactResponseDTO>(entity);
        }

        // GET ALL
        public async Task<IEnumerable<ContactResponseDTO>> GetAllContactsAsync()
        {
            var contacts = await _repository.GetAllContactsAsync();

            var activeContacts = contacts.Where(x => !x.IsDeleted);

            return _mapper.Map<IEnumerable<ContactResponseDTO>>(activeContacts);
        }

        // GET BY EMAIL
        public async Task<ContactResponseDTO?> GetContactByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var contact = await _repository.GetByEmailAsync(email);

            if (contact == null || contact.IsDeleted)
                return null;

            return _mapper.Map<ContactResponseDTO>(contact);
        }

        // UPDATE
        public async Task<ContactResponseDTO> UpdateContactAsync(
            string email,
            UpdateContactDTO updateRequest)
        {
            var existing = await _repository.GetByEmailAsync(email);

            if (existing == null || existing.IsDeleted)
                throw new Exception("Contact not found");

            _mapper.Map(updateRequest, existing);

            await _context.SaveChangesAsync();

            // FIX: Return Response DTO instead of Update DTO
            return _mapper.Map<ContactResponseDTO>(existing);
        }

        // DELETE
        public async Task<bool> DeleteContactAsync(string email)
        {
            var existing = await _repository.GetByEmailAsync(email);

            if (existing == null || existing.IsDeleted)
                return false;

            existing.IsDeleted = true;

            _context.Contacts.Update(existing);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}