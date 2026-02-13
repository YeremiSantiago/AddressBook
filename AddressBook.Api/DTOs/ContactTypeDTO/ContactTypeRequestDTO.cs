using System.ComponentModel.DataAnnotations;

namespace AddressBook.Api.DTOs.ContactTypeDTO
{
    public class ContactTypeRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
    }
}
