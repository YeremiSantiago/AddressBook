using AddressBook.Api.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AddressBook.Api.DTOs
{
    [SwaggerSchema("Contact information")]
    public abstract class ContactBaseDTO
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

        [Required]
        public int ContactTypeId { get; set; }
    }

    public class ContactReadDTO : ContactBaseDTO
    {
        public string ContactTypeName { get; set; }
        public DateOnly CreationDate { get; set; }
        public DateOnly? UpdateDate { get; set; }

    }

    public class ContactRequestDTO : ContactBaseDTO
    {
        private new int Id { get; set; }
    }
}
