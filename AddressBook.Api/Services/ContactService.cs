using AddressBook.Api.DTOs;
using AddressBook.Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AddressBook.Api.Services
{
    public class ContactService
    {
        private readonly AppDbContext _context;
        private IMapper _mapper;
        public ContactService
        (
            AppDbContext context, 
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ContactReadDTO> GetAllContacts()
        {
            return _mapper.Map<List<ContactReadDTO>>(_context.Contacts.Include(c => c.ContactType).ToList());
        }

        public ContactReadDTO GetContactById(int id)
        {
            return _mapper.Map<ContactReadDTO>(_context.Contacts.Include(c => c.ContactType).FirstOrDefault(c => c.Id == id));
        }

        public Contact AddContact(ContactRequestDTO contactDto)
        {
            var contact = _mapper.Map<Contact>(contactDto);
            _context.Contacts.Add(contact);
            _context.SaveChanges();

            return contact;
        }

        public void UpdateContact(int id, ContactRequestDTO contactDto)
        {
            var contact = _context.Contacts.Find(id);

            _mapper.Map(contactDto, contact);
            _context.SaveChanges();
        }

        public void DeleteContact(int id)
        {
            var contact = _context.Contacts.Find(id);

            _context.Contacts.Remove(contact);
            _context.SaveChanges();
        }

        public bool ContactWithSameNameExistsAsync(string name, string lastName)
        {
            return _context.Contacts.Any(x => x.FirstName == name && x.LastName == lastName);
        }

    }
}
