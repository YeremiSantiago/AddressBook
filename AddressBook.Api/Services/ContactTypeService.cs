using AddressBook.Api.DTOs;
using AddressBook.Api.DTOs.ContactTypeDTO;
using AddressBook.Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Api.Services
{
    public class ContactTypeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactTypeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContactTypeReadDTO>> GetAllContactTypeAsync()
        {
            var contactTypes = await _context.ContactTypes.ToListAsync();
            return _mapper.Map<IEnumerable<ContactTypeReadDTO>>(contactTypes);
        }

        public async Task<ContactTypeReadDTO?> GetContactTypeByIdAsync(int id)
        {
            var contactType = await _context.ContactTypes.FindAsync(id);
            return _mapper.Map<ContactTypeReadDTO>(contactType);
        }

        public async Task<ContactTypeReadDTO> CreateContactTypeAsync(ContactTypeRequestDTO request)
        {
            var contactType = _mapper.Map<ContactType>(request);
            
            await _context.ContactTypes.AddAsync(contactType);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<ContactTypeReadDTO>(contactType);
        }

        public async Task UpdateContactTypeAsync(int id, ContactTypeRequestDTO request)
        {
            var existingContactType = await _context.ContactTypes.FindAsync(id);

            if (existingContactType != null)
            {
                existingContactType.Name = request.Name;
                existingContactType.Description = request.Description;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteContactTypeAsync(int id)
        {
            var contactType = await _context.ContactTypes.FindAsync(id);

            if (contactType != null)
            {
                _context.ContactTypes.Remove(contactType);
                await _context.SaveChangesAsync();
            }
        }

        public bool ContactTypeExistsAsync(int id)
        {
            return _context.ContactTypes.Any(x => x.Id == id);
        }
    }
}
