using FakeStoreDBAPI.Host.DTO.Address;
using FakeStoreDBAPI.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FakeStoreDBAPI.Host.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var addresses = await _addressService.GetAllAsync();
            return Ok(addresses);
        }

        [HttpGet("{id}", Name = "GetAddressById")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long id)
        {
            var address = await _addressService.GetByIdAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateAddressDto addressDto)
        {
            var createdAddress = await _addressService.PostAsync(addressDto);
            return CreatedAtRoute("GetAddressById", new { id = createdAddress.Id }, createdAddress);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody] UpdateAddressDto address)
        {
            await _addressService.PatchAsync(id, address);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            await _addressService.DeleteAsync(id);
            return NoContent();
        }
    }
}
