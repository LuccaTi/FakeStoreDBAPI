using AutoMapper;
using FakeStoreDBAPI.Host.Data;
using FakeStoreDBAPI.Host.DTO.Customer;
using FakeStoreDBAPI.Host.Exceptions;
using FakeStoreDBAPI.Host.Models;
using FakeStoreDBAPI.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FakeStoreDBAPI.Host.Services
{
    public class CustomerService : ICustomerService
    {
        private const string _className = "CustomerService";
        private readonly FakeStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;
        private readonly IAddressService _addressService;

        public CustomerService(FakeStoreDbContext context, IMapper mapper, ILogger<CustomerService> logger, IAddressService addressService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _addressService = addressService;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            _logger.LogDebug($"{_className} - Attempting to obtain all customer records");
            var customers = await _context.Customers.Where(o => o.IsActive).ToListAsync();
            if (customers.Count != 0)
            {
                _logger.LogDebug($"{_className} - Records obtained: {customers.Count}");
            }
            else
            {
                _logger.LogWarning($"{_className} - List of records is empty");
            }
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public async Task<CustomerDto?> GetByIdAsync(long id)
        {
            _logger.LogDebug($"{_className} - Attempting to find customer ID: {id}");
            if (id == 0)
                throw new InvalidIdException($"Customer ID cannot be zero!");

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null || !customer.IsActive)
                throw new NotFoundException($"Customer ID: {id} was not found");

            _logger.LogDebug($"{_className} - Found customer ID: {id}");
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerWithAddressDto?> GetByIdWithAddressAsync(long id)
        {
            _logger.LogDebug($"{_className} - Attempting to find customer ID: {id} and return it with address info");
            if (id == 0)
                throw new InvalidIdException($"Customer ID cannot be zero!");

            var customer = await _context.Customers
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null || !customer.IsActive)
                throw new NotFoundException($"Customer ID: {id} was not found");

            _logger.LogDebug($"{_className} - Found customer ID: {id}");
            return _mapper.Map<CustomerWithAddressDto>(customer);
        }

        public async Task<CustomerDto> PostAsync(CreateCustomerDto customerDto)
        {
            _logger.LogDebug($"{_className} - Attempting to post customer");
            var customer = _mapper.Map<Customer>(customerDto);

            await _addressService.AddressExistsAsync(customerDto.AddressId);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var postedCustomer = _mapper.Map<CustomerDto>(customer);
            _logger.LogDebug($"{_className} - Posted customer ID: {postedCustomer.Id}");
            return postedCustomer;
        }

        public async Task PatchAsync(long id, UpdateCustomerDto customerDto)
        {
            _logger.LogDebug($"{_className} - Attempting to patch customer ID: {id}");

            var customerToUpdate = await _context.Customers.FindAsync(id);
            if (customerToUpdate == null || !customerToUpdate.IsActive)
                throw new NotFoundException($"Customer ID: {id} not found");

            if (customerDto.AddressId.HasValue)
            {
                _logger.LogDebug($"{_className} - Client provided address ID. Validating ID: {customerDto.AddressId.Value}");
                if (customerDto.AddressId.Value == 0)
                    throw new InvalidIdException("Address ID cannot be zero");

                await _addressService.AddressExistsAsync(customerDto.AddressId.Value);

                customerToUpdate.AddressId = customerDto.AddressId.Value;
                _logger.LogDebug($"Customer ID: {id} address ID patched to: {customerDto.AddressId.Value}");
            }

            _mapper.Map(customerDto, customerToUpdate);
            await _context.SaveChangesAsync();
            _logger.LogDebug($"{_className} - Patched customer ID: {id}");
        }

        public async Task DeleteAsync(long id)
        {
            _logger.LogDebug($"{_className} - Attempting to deactive customer ID: {id}");
            if (id == 0)
                throw new InvalidIdException($"Customer ID cannot be zero!");

            var customerToDelete = await _context.Customers.FindAsync(id);
            if (customerToDelete == null || !customerToDelete.IsActive)
                throw new NotFoundException($"Customer ID: {id} was not found, customer not deactivated");

            customerToDelete.IsActive = false;
            await _context.SaveChangesAsync();
            _logger.LogDebug($"{_className} - Successfully deactivated customer ID: {id}");
        }

        public async Task CustomerExistsAsync(long id)
        {
            _logger.LogDebug($"{_className} - Checking if customer ID: {id} exists and is active");
            if (id == 0)
                throw new InvalidIdException("Customer ID cannot be zero!");

            bool customerExists = false;
            customerExists = await _context.Customers.AnyAsync(c => c.Id == id && c.IsActive);
            if (!customerExists)
                throw new NotFoundException($"Customer ID: {id} does not exists or is inactive");

            _logger.LogDebug($"{_className} - Customer exists and is active");
        }
    }
}
