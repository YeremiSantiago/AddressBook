using Microsoft.EntityFrameworkCore;

namespace AddressBook.Api.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactType>().HasData(
                new ContactType { Id = 1, Name = "Family", Description = "Family members" },
                new ContactType { Id = 2, Name = "Friends", Description = "Friends and acquaintances" },
                new ContactType { Id = 3, Name = "Work", Description = "Work colleagues" }
            );
        }
    }
}
