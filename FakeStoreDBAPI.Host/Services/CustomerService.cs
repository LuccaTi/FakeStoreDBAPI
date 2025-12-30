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

        public async Task<IEnumerable<CustomerDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetAllAsync - Attempting to obtain all customer records");
            var customers = await _context.Customers.Where(o => o.IsActive).ToListAsync(cancellationToken);
            if (customers.Count != 0)
            {
                _logger.LogDebug($"{_className} - GetAllAsync - Records obtained: {customers.Count}");
            }
            else
            {
                _logger.LogWarning($"{_className} - GetAllAsync - List of records is empty");
            }
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public async Task<IEnumerable<CustomerDto>> GetAllActiveOrNotAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetAllActiveOrNotAsync - Attempting to obtain all customer records including the inactives");
            var customers = await _context.Customers.ToListAsync(cancellationToken);
            if (customers.Count != 0)
            {
                _logger.LogDebug($"{_className} - GetAllActiveOrNotAsync - Records obtained: {customers.Count}");
            }
            else
            {
                _logger.LogWarning($"{_className} - GetAllActiveOrNotAsync - List of records is empty");
            }

            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public async Task<CustomerDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetByIdAsync - Attempting to find customer with ID: {id}");
            if (id == 0)
                throw new InvalidIdException($"Customer ID cannot be zero!");

            var customer = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);
            if (customer == null || !customer.IsActive)
                throw new NotFoundException($"Customer with ID: {id} was not found!");

            _logger.LogDebug($"{_className} - GetByIdAsync - Found customer with ID: {id}");
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerWithAddressDto?> GetByIdWithAddressAsync(long id, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetByIdWithAddressAsync - Attempting to find customer with ID: {id} and return it with address info");
            if (id == 0)
                throw new InvalidIdException($"Customer ID cannot be zero!");

            var customer = await _context.Customers
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (customer == null)
                throw new NotFoundException($"Customer with ID: {id} was not found!");

            _logger.LogDebug($"{_className} - GetByIdWithAddressAsync - Found customer with ID: {id}");
            return _mapper.Map<CustomerWithAddressDto>(customer);
        }

        public async Task<CustomerDto?> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - LoginAsync - Attempting to obtain customer by using username: '{loginRequestDto.Username}' and it's password");
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserName == loginRequestDto.Username && c.IsActive, cancellationToken);
            if (customer == null)
                throw new NotFoundException($"Customer with username: '{loginRequestDto.Username}' was not found!");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, customer.Password);
            if (isPasswordValid)
            {
                _logger.LogDebug($"{_className} - LoginAsync - Found customer with username: '{loginRequestDto.Username}'");
                return _mapper.Map<CustomerDto>(customer);
            }
            else
            {
                _logger.LogWarning($"{_className} - LoginAsync - Authentication failed for user: '{loginRequestDto.Username}'");
                throw new InvalidResourceException($"Invalid username or password!");
            }
        }

        public async Task<CustomerDto> PostAsync(CreateCustomerDto customerDto, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - PostAsync - Attempting to post customer");
            var customer = _mapper.Map<Customer>(customerDto);
            customer.Password = BCrypt.Net.BCrypt.HashPassword(customerDto.Password);

            bool customerExists = await _context.Customers.AnyAsync(c => c.UserName == customer.UserName);
            if (customerExists)
                throw new ConflictException($"Customer with username: '{customer.UserName}' already posted!");

            await _addressService.AddressExistsAsync(customerDto.AddressId, cancellationToken);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);

            var postedCustomer = _mapper.Map<CustomerDto>(customer);
            _logger.LogDebug($"{_className} - PostAsync - Posted customer with ID: {postedCustomer.Id}");
            return postedCustomer;
        }

        public async Task PatchAsync(long id, UpdateCustomerDto customerDto, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - PatchAsync - Attempting to patch customer with ID: {id}");

            var customerToUpdate = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);
            if (customerToUpdate == null || !customerToUpdate.IsActive)
                throw new NotFoundException($"Customer with ID: {id} was not found!");

            _mapper.Map(customerDto, customerToUpdate);

            if (customerDto.AddressId.HasValue)
            {
                _logger.LogDebug($"{_className} - PatchAsync - Client provided address ID. Validating ID: {customerDto.AddressId.Value}");
                if (customerDto.AddressId.Value == 0)
                    throw new InvalidIdException("Address ID cannot be zero!");

                await _addressService.AddressExistsAsync(customerDto.AddressId.Value, cancellationToken);

                customerToUpdate.AddressId = customerDto.AddressId.Value;
                _logger.LogDebug($"{_className} - PatchAsync - Customer with ID: {id} patched it's address ID to: {customerDto.AddressId.Value}");
            }
            else
            {
                _context.Entry(customerToUpdate).Property(x => x.AddressId).IsModified = false;
            }

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug($"{_className} - PatchAsync - Patched customer with ID: {id}");
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - DeleteAsync - Attempting to deactive customer with ID: {id}");
            if (id == 0)
                throw new InvalidIdException($"Customer ID cannot be zero!");

            var customerToDelete = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);
            if (customerToDelete == null || !customerToDelete.IsActive)
                throw new NotFoundException($"Customer with ID: {id} was not found, customer has not been deactivated!");

            customerToDelete.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug($"{_className} - DeleteAsync - Successfully deactivated customer with ID: {id}");
        }

        public async Task CustomerExistsAsync(long id, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - CustomerExistsAsync - Checking if customer with ID: {id} exists and is active");
            if (id == 0)
                throw new InvalidIdException("Customer ID cannot be zero!");

            bool customerExists = false;
            customerExists = await _context.Customers.AnyAsync(c => c.Id == id && c.IsActive, cancellationToken);
            if (!customerExists)
                throw new NotFoundException($"Customer with ID: {id} does not exists or is inactive!");

            _logger.LogDebug($"{_className} - CustomerExistsAsync - Customer with ID: {id} exists and is active");
        }
    }
}
