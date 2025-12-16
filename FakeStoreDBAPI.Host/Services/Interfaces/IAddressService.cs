using Microsoft.AspNetCore.Mvc;
using FakeStoreDBAPI.Host.Models;
using FakeStoreDBAPI.Host.DTO.Address;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IAddressService
    {
        public Task<IEnumerable<AddressDto>> GetAllAsync(CancellationToken cancellationToken);
        public Task<AddressDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
        public Task<AddressDto?> GetByStreetNumberAsync(StreetNumberDto streetNumberDto, CancellationToken cancellationToken);
        public Task<AddressDto> PostAsync(CreateAddressDto addressDto, CancellationToken cancellationToken);
        public Task PatchAsync(long id, UpdateAddressDto addressDto, CancellationToken cancellationToken);
        public Task DeleteAsync(long id, CancellationToken cancellationToken);
        public Task AddressExistsAsync(long id, CancellationToken cancellationToken);
    }
}
