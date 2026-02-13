using AddressBook.Api.DTOs;
using AddressBook.Api.DTOs.ContactTypeDTO;
using AddressBook.Api.Models;
using AddressBook.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AddressBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactTypeController : ControllerBase
    {
        private readonly ContactTypeService _contactTypeService;

        public ContactTypeController(ContactTypeService contactTypeService)
        {
            _contactTypeService = contactTypeService;
        }

        [HttpGet]
        [SwaggerOperation("Gets all contact types")]
        public async Task<ActionResult<IEnumerable<ContactTypeReadDTO>>> GetAllAsync()
        {
            return Ok(await _contactTypeService.GetAllContactTypeAsync());
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Gets a contact type by ID")]
        public async Task<ActionResult<ContactTypeReadDTO>> GetByIdAsync(int id)
        {
            var ExistedContactType = await _contactTypeService.GetContactTypeByIdAsync(id);

            if(ExistedContactType == null)
            {
                return NotFound();
            }
            return Ok(ExistedContactType);
        }

        [HttpPost]
        [SwaggerOperation("Creates a new contact type")]
        public async Task<ActionResult> CreateAsync([FromBody] ContactTypeRequestDTO request)
        {
            var createdContactT = await _contactTypeService.CreateContactTypeAsync(request);

            return StatusCode(201, createdContactT);

        }

        [SwaggerOperation("Updates an existing contact type")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] ContactTypeRequestDTO request)
        {
            var existing = await _contactTypeService.GetContactTypeByIdAsync(id);

            if(existing == null)
            {
                return NotFound();
            }

            await _contactTypeService.UpdateContactTypeAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Deletes a contact type")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var existing = await _contactTypeService.GetContactTypeByIdAsync(id);
            
            if(existing == null)
            {
                return NotFound();
            }

            await _contactTypeService.DeleteContactTypeAsync(id);
            return NoContent();
        }
    }
}
