using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressBook.Api.Models
{
    [Index(nameof(FirstName), nameof(LastName), IsUnique = true)]
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }
        

        [StringLength(200)]
        public string? Address { get; set; }

        [ForeignKey(nameof(ContactType))]
        public int ContactTypeId { get; set; }

        public DateOnly CreationDate { get; set; }

        public DateOnly UpdateDate { get; set; }

        public ContactType ContactType { get; set; }
    }
}
