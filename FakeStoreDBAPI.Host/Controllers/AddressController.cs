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
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var addresses = await _addressService.GetAllAsync(cancellationToken);
            return Ok(addresses);
        }

        [HttpGet("{id}", Name = "GetAddressById")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var address = await _addressService.GetByIdAsync(id, cancellationToken);
            return Ok(address);
        }

        [HttpHead("{id}/address-exists")]
        public async Task<IActionResult> ExistsAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            await _addressService.AddressExistsAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("street-number")]
        public async Task<IActionResult> GetByStreetNumberAsync([FromBody] StreetNumberDto streetNumberDto, CancellationToken cancellationToken)
        {
            var address = await _addressService.GetByStreetNumberAsync(streetNumberDto, cancellationToken);
            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateAddressDto addressDto, CancellationToken cancellationToken)
        {
            var createdAddress = await _addressService.PostAsync(addressDto, cancellationToken);
            return CreatedAtRoute("GetAddressById", new { id = createdAddress.Id }, createdAddress);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody] UpdateAddressDto addressDto, CancellationToken cancellationToken)
        {
            await _addressService.PatchAsync(id, addressDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            await _addressService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
