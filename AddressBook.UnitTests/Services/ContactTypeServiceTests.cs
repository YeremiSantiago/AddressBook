using AddressBook.Api.DTOs.ContactTypeDTO;
using AddressBook.Api.Models;
using AddressBook.Api.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AddressBook.UnitTests.Services
{
    public class ContactTypeServiceTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ContactTypeService _service;

        public ContactTypeServiceTests()
        {
            // Configuracion de la  base de datos en memoria .
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Mock 
            _mapperMock = new Mock<IMapper>();

            _service = new ContactTypeService(_context, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllContactTypeAsync_ReturnsAllContactTypes()
        {
            // Arrange
            var contactTypes = new List<ContactType>
            {
                new ContactType { Id = 1, Name = "Family", Description = "Family members" },
                new ContactType { Id = 2, Name = "Friends", Description = "Friends" }
            };

            _context.ContactTypes.AddRange(contactTypes);
            await _context.SaveChangesAsync();

            var expectedDtos = new List<ContactTypeReadDTO>
            {
                new ContactTypeReadDTO { Id = 1, Name = "Family", Description = "Family members" },
                new ContactTypeReadDTO { Id = 2, Name = "Friends", Description = "Friends" }
            };

            _mapperMock.Setup(m => m.Map<IEnumerable<ContactTypeReadDTO>>(It.IsAny<List<ContactType>>()))
                .Returns(expectedDtos);

            // Act
            var result = await _service.GetAllContactTypeAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, ct => ct.Name == "Family");
            Assert.Contains(result, ct => ct.Name == "Friends");
        }

        [Fact]
        public async Task GetContactTypeByIdAsync_ExistingId_ReturnsContactType()
        {
            // Arrange
            var contactType = new ContactType { Id = 1, Name = "Work", Description = "Work contacts" };
            _context.ContactTypes.Add(contactType);
            await _context.SaveChangesAsync();

            var expectedDto = new ContactTypeReadDTO { Id = 1, Name = "Work", Description = "Work contacts" };

            _mapperMock.Setup(m => m.Map<ContactTypeReadDTO>(It.IsAny<ContactType>()))
                .Returns(expectedDto);

            // Act
            var result = await _service.GetContactTypeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Work", result.Name);
            Assert.Equal("Work contacts", result.Description);
        }

        [Fact]
        public async Task GetContactTypeByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            _mapperMock.Setup(m => m.Map<ContactTypeReadDTO>(null))
                .Returns((ContactTypeReadDTO)null);

            // Act
            var result = await _service.GetContactTypeByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateContactTypeAsync_ValidRequest_CreatesContactType()
        {
            // Arrange
            var request = new ContactTypeRequestDTO
            {
                Name = "Business",
                Description = "Business contacts"
            };

            var contactType = new ContactType { Id = 1, Name = "Business", Description = "Business contacts" };
            var expectedDto = new ContactTypeReadDTO { Id = 1, Name = "Business", Description = "Business contacts" };

            _mapperMock.Setup(m => m.Map<ContactType>(It.IsAny<ContactTypeRequestDTO>()))
                .Returns(contactType);

            _mapperMock.Setup(m => m.Map<ContactTypeReadDTO>(It.IsAny<ContactType>()))
                .Returns(expectedDto);

            // Act
            var result = await _service.CreateContactTypeAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Business", result.Name);
            Assert.Equal("Business contacts", result.Description);
            
            var savedContactType = await _context.ContactTypes.FirstOrDefaultAsync();
            Assert.NotNull(savedContactType);
            Assert.Equal("Business", savedContactType.Name);
        }

        [Fact]
        public async Task UpdateContactTypeAsync_ExistingId_UpdatesContactType()
        {
            // Arrange
            var contactType = new ContactType { Id = 1, Name = "Old Name", Description = "Old Description" };
            _context.ContactTypes.Add(contactType);
            await _context.SaveChangesAsync();

            var request = new ContactTypeRequestDTO
            {
                Name = "New Name",
                Description = "New Description"
            };

            // Act
            await _service.UpdateContactTypeAsync(1, request);

            // Assert
            var updatedContactType = await _context.ContactTypes.FindAsync(1);
            Assert.NotNull(updatedContactType);
            Assert.Equal("New Name", updatedContactType.Name);
            Assert.Equal("New Description", updatedContactType.Description);
        }

        [Fact]
        public async Task UpdateContactTypeAsync_NonExistingId_DoesNotThrowException()
        {
            // Arrange
            var request = new ContactTypeRequestDTO
            {
                Name = "New Name",
                Description = "New Description"
            };

            // Act & Assert
            await _service.UpdateContactTypeAsync(999, request);
           
        }

        [Fact]
        public async Task DeleteContactTypeAsync_ExistingId_DeletesContactType()
        {
            // Arrange
            var contactType = new ContactType { Id = 1, Name = "To Delete", Description = "Will be deleted" };
            _context.ContactTypes.Add(contactType);
            await _context.SaveChangesAsync();

            // Act
            await _service.DeleteContactTypeAsync(1);

            // Assert
            var deletedContactType = await _context.ContactTypes.FindAsync(1);
            Assert.Null(deletedContactType);
        }

        [Fact]
        public async Task DeleteContactTypeAsync_NonExistingId_DoesNotThrowException()
        {
            // Act & Assert
            await _service.DeleteContactTypeAsync(999);
            
        }

        [Fact]
        public void ContactTypeExistsAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            var contactType = new ContactType { Id = 1, Name = "Existing", Description = "Exists" };
            _context.ContactTypes.Add(contactType);
            _context.SaveChanges();

            // Act
            var result = _service.ContactTypeExistsAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ContactTypeExistsAsync_NonExistingId_ReturnsFalse()
        {
            // Act
            var result = _service.ContactTypeExistsAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}