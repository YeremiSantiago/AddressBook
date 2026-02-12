using System.ComponentModel.DataAnnotations;

namespace AddressBook.Api.Models
{
    public class ContactType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public ICollection<Contact> Contacts { get; set; }
    }
}
