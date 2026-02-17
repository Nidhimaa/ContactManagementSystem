using ContactManagementSystem.Repositories.Interface;
using ContactManagementSystem.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagementSystem.Repositories.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;

        public ContactRepository(AppDbContext context)
        {
            _context = context;
        }

        // FETCH ALL
        public async Task<List<ContactManagement>> GetAllContactsAsync()
        {
            return await _context.Contacts
                .AsNoTracking()
                .ToListAsync();
        }

        // FETCH BY EMAIL
        public async Task<ContactManagement?> GetByEmailAsync(string email)
        {
            return await _context.Contacts
                .FirstOrDefaultAsync(x =>
                    x.Email.ToLower() == email.ToLower());
        }

        // VERIFY EMAIL EXISTS
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Contacts
                .AnyAsync(x =>
                    x.Email.ToLower() == email.ToLower()
                    && !x.IsDeleted);
        }
    }
}