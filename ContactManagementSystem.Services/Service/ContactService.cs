using AutoMapper;
using ContactManagementSystem.DTOs;
using ContactManagementSystem.Repositories;
using ContactManagementSystem.Repositories.Interface;
using ContactManagementSystem.Repositories.Models;
using ContactManagementSystem.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ContactManagementSystem.Services.Service
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuditService _auditService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactService(
            IContactRepository repository,
            AppDbContext context,
            IMapper mapper,
            IAuditService auditService,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
            _auditService = auditService;
            _httpContextAccessor = httpContextAccessor;
        }

        // Helper method to get logged user email
        private string GetLoggedUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User
                ?.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown";
        }

        // CREATE
        public async Task<ContactResponseDTO> AddContactAsync(
            ContactRequestDTO request)
        {
            var entity = _mapper.Map<ContactManagement>(request);

            entity.IsDeleted = false;

            _context.Contacts.Add(entity);
            await _context.SaveChangesAsync();

            // Audit Log for create Contact.
            await _auditService.LogAuditEventAsync(new AuditEventDTO
            {
                UserAction = "Contact Created",
                UserEmail = GetLoggedUserEmail(),
                ContactId = entity.Id
            });

            return _mapper.Map<ContactResponseDTO>(entity);
        }

        // GET ALL
        public async Task<List<ContactResponseDTO>> GetAllContactsAsync()
        {
            var contacts = await _repository.GetAllContactsAsync();

            var activeContacts = contacts.Where(x => !x.IsDeleted);

            return _mapper.Map<List<ContactResponseDTO>>(activeContacts);
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

            // Audit Log for update Contact.
            await _auditService.LogAuditEventAsync(new AuditEventDTO
            {
                UserAction = "Contact Updated",
                UserEmail = GetLoggedUserEmail(),
                ContactId = existing.Id
            });

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

            await _auditService.LogAuditEventAsync(new AuditEventDTO
            {
                UserAction = "Contact Deleted",
                UserEmail = GetLoggedUserEmail(),
                ContactId = existing.Id
            });

            return true;
        }
    }
}