using Microsoft.AspNetCore.Mvc;
using FakeStoreDBAPI.Host.Services.Interfaces;
using FakeStoreDBAPI.Host.Models;
using FakeStoreDBAPI.Host.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FakeStoreDBAPI.Host.DTO.Address;
using FakeStoreDBAPI.Host.Exceptions;

namespace FakeStoreDBAPI.Host.Services
{
    public class AddressService : IAddressService
    {
        private const string _className = "AddressService";
        private readonly FakeStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;

        public AddressService(FakeStoreDbContext context, IMapper mapper, ILogger<AddressService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AddressDto>> GetAllAsync()
        {
            _logger.LogDebug($"{_className} - GetAllAsync -Attempting to obtain all address records");
            var addresses = await _context.Addresses.Where(o => o.IsActive).ToListAsync();
            if (addresses.Count != 0)
            {
                _logger.LogDebug($"{_className} - GetAllAsync - Records obtained: {addresses.Count}");
            }
            else
            {
                _logger.LogWarning($"{_className} - GetAllAsync - List of records is empty!");
            }
            return _mapper.Map<IEnumerable<AddressDto>>(addresses);
        }

        public async Task<AddressDto?> GetByIdAsync(long id)
        {
            _logger.LogDebug($"{_className} - GetByIdAsync - Attempting to find address with ID: {id}");
            if (id == 0)
                throw new InvalidIdException($"Address ID cannot be zero!");

            var address = await _context.Addresses.FindAsync(id);
            if (address == null || !address.IsActive)
                throw new NotFoundException($"Address with ID: {id} was not found");

            _logger.LogDebug($"{_className} - GetByIdAsync - Found address with ID: {id}");
            return _mapper.Map<AddressDto>(address);
        }

        public async Task<AddressDto> PostAsync(CreateAddressDto addressDto)
        {
            _logger.LogDebug($"{_className} - PostAsync - Attempting to post address");
            var address = _mapper.Map<Address>(addressDto);
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            var postedAddress = _mapper.Map<AddressDto>(address);
            _logger.LogDebug($"{_className} - PostAsync - Posted address with ID: {postedAddress.Id}");
            return postedAddress;
        }

        public async Task PatchAsync(long id, UpdateAddressDto addressDto)
        {
            _logger.LogDebug($"{_className} - PatchAsync - Attempting to patch address with ID: {id}");
            if (id == 0)
                throw new InvalidIdException("Address ID cannot be zero!");

            var addressToUpdate = await _context.Addresses.FindAsync(id);
            if (addressToUpdate == null || !addressToUpdate.IsActive)
                throw new NotFoundException($"Address with ID: {id} not found");

            _mapper.Map(addressDto, addressToUpdate);
            await _context.SaveChangesAsync();
            _logger.LogDebug($"{_className} - PatchAsync - Patched address with ID: {id}");
        }

        public async Task DeleteAsync(long id)
        {
            _logger.LogDebug($"{_className} - DeleteAsync - Attempting to deactivate address with ID: {id} and its dependencies");
            if (id == 0)
                throw new InvalidIdException($"Address ID cannot be zero!");

            var addressToDelete = await _context.Addresses.FindAsync(id);
            if (addressToDelete == null || !addressToDelete.IsActive)
                throw new NotFoundException($"Address with ID: {id} was not found, address not deactivated");

            _logger.LogDebug($"{_className} - DeleteAsync - Checking if address had any customers associated");
            var customersToDeactivate = await _context.Customers
                .Where(c => c.AddressId == id && c.IsActive)
                .ToListAsync();

            if (customersToDeactivate.Any())
            {
                foreach (var customer in customersToDeactivate)
                {
                    customer.IsActive = false;
                }
                _logger.LogDebug($"{_className} - DeleteAsync - Associated customers deactivated: {customersToDeactivate.Count}");
            }
            else
            {
                _logger.LogDebug($"{_className} - DeleteAsync - Address didn't have any customers associated with it");
            }
            addressToDelete.IsActive = false;
            await _context.SaveChangesAsync();
            _logger.LogDebug($"{_className} - DeleteAsync - Successfully deactivated address with ID: {id} ");
        }

        public async Task AddressExistsAsync(long id)
        {
            _logger.LogDebug($"{_className} - AddressExistsAsync - Checking if address with ID: {id} exists and is active");
            if (id == 0)
                throw new InvalidIdException("Address ID cannot be zero!");

            bool addressExists = false;
            addressExists = await _context.Addresses.AnyAsync(a => a.Id == id && a.IsActive);
            if (!addressExists)
                throw new NotFoundException($"Address with ID: {id} does not exists or is inactive");

            _logger.LogDebug($"{_className} - AddressExistsAsync - Address exists and is active");
        }
    }
}
