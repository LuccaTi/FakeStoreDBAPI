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
            var customers = await _context.Customers.ToListAsync();
            if (customers.Count != 0)
            {
                _logger.LogDebug($"{_className} - Records obtained");
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
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                throw new NotFoundException($"Customer ID: {id} was not found");
            }

            _logger.LogDebug($"{_className} - Found customer ID: {id}");
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerWithAddressDto?> GetByIdWithAddressAsync(long id)
        {
            _logger.LogDebug($"{_className} - Attempting to find customer ID: {id} and return it with address info");
            var customer = await _context.Customers
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null || !customer.IsActive)
            {
                throw new NotFoundException($"Customer ID: {id} was not found");
            }

            _logger.LogDebug($"{_className} - Found customer ID: {id}");
            return _mapper.Map<CustomerWithAddressDto>(customer);
        }

        public async Task<CustomerDto> PostAsync(CreateCustomerDto customerDto)
        {
            _logger.LogDebug($"{_className} - Attempting to post new customer");
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
            if (customerDto.AddressId == 0)
            {
                throw new InvalidIdException($"Address id provided cannot be zero!");
            }

            _logger.LogDebug($"{_className} - Cheking if address ID: {customerDto.AddressId} is valid");
            await _addressService.AddressExistsAsync(customerDto.AddressId);

            var customerToUpdate = await _context.Customers.FindAsync(id);
            if (customerToUpdate == null || !customerToUpdate.IsActive)
            {
                throw new NotFoundException($"Customer ID: {id} not found");
            }

            _mapper.Map(customerDto, customerToUpdate);
            _logger.LogDebug($"{_className} - Patched customer ID: {id}");

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            _logger.LogDebug($"{_className} - Attempting to deactive customer ID: {id}");
            var customerToDelete = await _context.Customers.FindAsync(id);
            if (customerToDelete == null || !customerToDelete.IsActive)
            {
                throw new NotFoundException($"Customer ID: {id} was not found, customer not deactivated");
            }

            customerToDelete.IsActive = false;
            await _context.SaveChangesAsync();
            _logger.LogDebug($"{_className} - Customer ID: {id} successfully deactivated");
        }
    }
}
