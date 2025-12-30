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
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var customers = await _customerService.GetAllAsync(cancellationToken);
            return Ok(customers);
        }

        [HttpGet("active-or-not")]
        public async Task<IActionResult> GetAllActiveOrNotAsync(CancellationToken cancellationToken)
        {
            var customers = await _customerService.GetAllActiveOrNotAsync(cancellationToken);
            return Ok(customers);
        }

        [HttpGet("{id}", Name = "GetCustomerById")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetByIdAsync(id, cancellationToken);
            return Ok(customer);
        }

        [HttpGet("{id}/with-address")]
        public async Task<IActionResult> GetByIdWithAddressAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetByIdWithAddressAsync(id, cancellationToken);
            return Ok(customer);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
        {
            var customer = await _customerService.LoginAsync(loginRequestDto, cancellationToken);
            return Ok(customer);
        }

        [HttpHead("{id}/customer-exists")]
        public async Task<IActionResult> ExistsAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            await _customerService.CustomerExistsAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateCustomerDto customerDto, CancellationToken cancellationToken)
        {
            var createdCustomer = await _customerService.PostAsync(customerDto, cancellationToken);
            return CreatedAtRoute("GetCustomerById", new { id = createdCustomer.Id }, createdCustomer);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody] UpdateCustomerDto customerDto, CancellationToken cancellationToken)
        {
            await _customerService.PatchAsync(id, customerDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            await _customerService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
