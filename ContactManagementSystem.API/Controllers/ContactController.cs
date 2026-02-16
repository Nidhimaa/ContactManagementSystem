using ContactManagementSystem.API.Validators;
using ContactManagementSystem.DTOs;
using ContactManagementSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _service;
        private readonly ContactValidator _validator;
        private readonly UpdateValidator _updateValidator;

        public ContactController(
            IContactService service,
            ContactValidator validator,
            UpdateValidator updatevalidator)
        {
            _service = service;
            _validator = validator;
            _updateValidator = updatevalidator;
        }

        // CREATE CONTACT
        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContactRequestDTO request)
        {
            try
            {
                var validation = await _validator.ValidateAsync(request);

                if (!validation.IsValid)
                    return BadRequest(new
                    {
                        Errors = validation.Errors.Select(x => x.ErrorMessage)
                    });


                var result = await _service.AddContactAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET ALL CONTACTS
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            try
            {
                var result = await _service.GetAllContactsAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET CONTACT BY EMAIL
        [HttpGet("{email}")]
        public async Task<IActionResult> GetContactByEmail(string email)
        {
            try
            {
                var result = await _service.GetContactByEmailAsync(email);

                if (result == null)
                    return NotFound("Contact not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // UPDATE CONTACT BY EMAIL
        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateContact(
            string email,
            [FromBody] UpdateContactDTO updateRequest)
        {
            try
            {
                var validation = await _updateValidator.ValidateAsync(updateRequest);

                if (!validation.IsValid)
                    return BadRequest(new
                    {
                        Errors = validation.Errors.Select(x => x.ErrorMessage)
                    });

                var result = await _service.UpdateContactAsync(email, updateRequest);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE CONTACT
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteContact(string email)
        {
            try
            {
                var deleted = await _service.DeleteContactAsync(email);

                if (!deleted)
                    return NotFound("Contact not found or already deleted");

                return Ok("Contact deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}