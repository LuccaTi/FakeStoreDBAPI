using FakeStoreDBAPI.Host.DTO.Customer;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<IEnumerable<CustomerDto>> GetAllAsync();
        public Task<CustomerDto?> GetByIdAsync(long id);
        public Task<CustomerWithAddressDto?> GetByIdWithAddressAsync(long id);
        public Task<CustomerDto?> LoginAsync(LoginRequestDto loginRequestDto); 
        public Task<CustomerDto> PostAsync(CreateCustomerDto customerDto);
        public Task PatchAsync(long id, UpdateCustomerDto customerDto);
        public Task DeleteAsync(long id);
        public Task CustomerExistsAsync(long id);
    }
}
