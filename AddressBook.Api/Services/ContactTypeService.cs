using AddressBook.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Api.Services
{
    public class ContactTypeService
    {
        private readonly AppDbContext _context;

        public ContactTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactType>> GetAllContactTypeAsync()
        {
            return await _context.ContactTypes.ToListAsync();
        }

        public async Task<ContactType?> GetContactTypeByIdAsync(int id)
        {
            return await _context.ContactTypes.FindAsync(id);
        }

        public async Task<ContactType> CreateContactTypeAsync(ContactType contactType)
        { 
            await _context.ContactTypes.AddAsync(contactType);
            await _context.SaveChangesAsync();
            return contactType;
        }

        public async Task UpdateContactTypeAsync(int id, ContactType contactType)
        {
            var existingContactType = await _context.ContactTypes.FindAsync(id);

            if (existingContactType != null)
            {
                existingContactType.Description = contactType.Description;
                existingContactType.Name = contactType.Name;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteContactTypeAsync(int id)
        {
            var existingContactType = await _context.ContactTypes.FindAsync(id);

            if (existingContactType != null)
            {
                _context.ContactTypes.Remove(existingContactType);
                await _context.SaveChangesAsync();

            }

        }

    }
}
