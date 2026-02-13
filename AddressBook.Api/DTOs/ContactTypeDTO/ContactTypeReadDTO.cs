using System.ComponentModel.DataAnnotations;

namespace AddressBook.Api.DTOs.ContactTypeDTO
{
    public class ContactTypeReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

   
}
