using Microsoft.AspNetCore.Mvc;
using FakeStoreDBAPI.Host.Models;
using FakeStoreDBAPI.Host.DTO.Address;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IAddressService
    {
        public Task<IEnumerable<AddressDto>> GetAllAsync();
        public Task<AddressDto?> GetByIdAsync(long id);
        public Task<AddressDto> PostAsync(CreateAddressDto addressDto);
        public Task PatchAsync(long id, UpdateAddressDto addressDto);
        public Task DeleteAsync(long id);
        public Task AddressExistsAsync(long id);
    }
}
