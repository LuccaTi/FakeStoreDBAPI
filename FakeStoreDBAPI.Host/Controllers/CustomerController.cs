using FakeStoreDBAPI.Host.DTO.Customer;
using FakeStoreDBAPI.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FakeStoreDBAPI.Host.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}", Name = "GetCustomerById")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            return Ok(customer);
        }

        [HttpGet("{id}/with-address")]
        public async Task<IActionResult> GetByIdWithAddressAsync([FromRoute] long id)
        {
            var customer = await _customerService.GetByIdWithAddressAsync(id);
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateCustomerDto customerDto)
        {
            var createdCustomer = await _customerService.PostAsync(customerDto);
            return CreatedAtRoute("GetCustomerById", new { id = createdCustomer.Id }, createdCustomer);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody] UpdateCustomerDto customerDto)
        {
            await _customerService.PatchAsync(id, customerDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            await _customerService.DeleteAsync(id);
            return NoContent();
        }
    }
}
