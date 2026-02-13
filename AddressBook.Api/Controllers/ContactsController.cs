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
            return Ok(_service.GetContactById(id));
        }


        [HttpPost]
        [SwaggerOperation("Creates a new contact")]
        public ActionResult Post([FromBody] ContactRequestDTO request)
        {
            var duplicateContactExists = _service.ContactWithSameNameExistsAsync(request.FirstName, request.LastName);

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
            return CreatedAtAction("Get", result.Id);
        }


        [HttpPut]
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
        [SwaggerOperation("Deletes a contact by id")]
        public void Delete(int id)
        {
            _service.DeleteContact(id);
        }
    }
}
