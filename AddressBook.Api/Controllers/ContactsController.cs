using AddressBook.Api.DTOs;
using AddressBook.Api.Models;
using AddressBook.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AddressBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactService _service;
        private readonly ContactTypeService _contactTypeService;
        public ContactsController
        (
            ContactService service,
            ContactTypeService contactTypeService
        )
        {
            _service = service;
            _contactTypeService = contactTypeService;
        }

        [HttpGet]
        [SwaggerOperation("Gets all contacts")]
        public ActionResult<IEnumerable<ContactReadDTO>> Get()
        {
            return Ok(_service.GetAllContacts());
        }


        [HttpGet("{id}")]
        [SwaggerOperation("Gets a contact by id")]
        public ActionResult<ContactReadDTO> Get(int id)
        {
            var contact = _service.GetContactById(id);

            if (contact == null)
            {
                return NotFound($"Contact with ID {id} not found.");
            }

            return Ok(contact);
        }

        [HttpGet("deleted")]
        [SwaggerOperation("Gets all deleted contacts")]
        public ActionResult<IEnumerable<ContactReadDTO>> GetAllDeleted()
        {
            return Ok(_service.GetAllDeletedContacts()); 
        }

        [HttpPost]
        [SwaggerOperation("Creates a new contact")]
        public ActionResult Post([FromBody] ContactRequestDTO request)
        {
            var duplicateContactExists = _service.ContactWithSameNameExists(request.FirstName, request.LastName);

            if (duplicateContactExists)
            {
                return BadRequest("A contact with the same first and last name already exists.");
            }

            var categoryTypeExists = _contactTypeService.ContactTypeExistsAsync(request.ContactTypeId);

            if (!categoryTypeExists)
            {
                return BadRequest($"ContactType with ID {request.ContactTypeId} does not exist.");
            }

            var result = _service.AddContact(request);
            return CreatedAtAction("Get", new { id = result.Id }, result);
        }


        [HttpPut("{id}")]
        [SwaggerOperation("Updates a contact by id")]
        public ActionResult Put(int id, [FromBody] ContactRequestDTO request)
        {
            var categoryTypeExists = _contactTypeService.ContactTypeExistsAsync(request.ContactTypeId);

            if (!categoryTypeExists)
            {
                return BadRequest($"ContactType with ID {request.ContactTypeId} does not exist.");
            }

            _service.UpdateContact(id, request);

            return NoContent();
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("Deletes a contact by id (soft delete)")]
        public ActionResult Delete(int id)
        {
            _service.DeleteContact(id);
            return NoContent();
        }

        [HttpPost("{id}/restore")]
        [SwaggerOperation("Restores a deleted contact")]
        public ActionResult Restore(int id)
        {
            var restored = _service.RestoreContact(id);

            if (!restored)
            {
                return NotFound($"Deleted contact with ID {id} not found.");
            }

            return Ok(new { message = "Contact restored successfully.", id });
        }

    }
}
