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
    internal class AddressService : IAddressService
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
            _logger.LogDebug($"{_className} - Attempting to obtain all address records");
            var addresses = await _context.Addresses.ToListAsync();
            if (addresses.Count != 0)
            {
                _logger.LogDebug($"{_className} - Records obtained");
            }
            else
            {
                _logger.LogDebug($"{_className} - List of records is empty!");
            }
            return _mapper.Map<IEnumerable<AddressDto>>(addresses);
        }

        public async Task<AddressDto?> GetByIdAsync(long id)
        {
            _logger.LogDebug($"{_className} - Attempting to find address with ID: {id}");
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                _logger.LogDebug($"{_className} - Found address with ID: {id}");
            }
            else
            {
                _logger.LogDebug($"{_className} - Address with ID: {id} was not found");
            }
            return _mapper.Map<AddressDto>(address);
        }

        public async Task<AddressDto> PostAsync(CreateAddressDto addressDto)
        {
            _logger.LogDebug($"{_className} - Attempting to post new address");
            var address = _mapper.Map<Address>(addressDto);
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            var postedAddress = _mapper.Map<AddressDto>(address);
            _logger.LogDebug($"{_className} - Posted address with ID: {postedAddress.Id}");
            return postedAddress;
        }

        public async Task PatchAsync(long id, UpdateAddressDto addressDto)
        {
            _logger.LogDebug($"{_className} - Attempting to patch address with ID: {id}");
            var addressToUpdate = await _context.Addresses.FindAsync(id);
            if (addressToUpdate == null)
            {
                throw new NotFoundException($"{_className} - Address with ID: {id} not found.");
            }

            _mapper.Map(addressDto, addressToUpdate);
            _logger.LogDebug($"{_className} - Patched address with ID: {id}", addressToUpdate.Id);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            _logger.LogDebug($"{_className} - Attempting to deactivate address with ID: {id}");
            var addressToDelete = await _context.Addresses.FindAsync(id);
            if (addressToDelete != null)
            {
                addressToDelete.IsActive = false;
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"{_className} - Address with ID: {id} was not found");
            }
        }
    }
}
