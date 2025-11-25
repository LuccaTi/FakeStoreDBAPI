using Microsoft.AspNetCore.Mvc;
using FakeStoreDBAPI.Host.Models;
using FakeStoreDBAPI.Host.DTO.Address;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IAddressService
    {
        public Task<IEnumerable<AddressDto>> GetAllAsync();
        public Task<AddressDto?> GetByIdAsync(long id);
        public Task<AddressDto> PostAsync(CreateAddressDto address);
        public Task PatchAsync(long id, UpdateAddressDto address);
        public Task DeleteAsync(long id);
    }
}
