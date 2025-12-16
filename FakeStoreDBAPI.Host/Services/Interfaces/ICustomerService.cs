using FakeStoreDBAPI.Host.DTO.Customer;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<IEnumerable<CustomerDto>> GetAllAsync(CancellationToken cancellationToken);
        public Task<CustomerDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
        public Task<CustomerWithAddressDto?> GetByIdWithAddressAsync(long id, CancellationToken cancellationToken);
        public Task<CustomerDto?> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken); 
        public Task<CustomerDto> PostAsync(CreateCustomerDto customerDto, CancellationToken cancellationToken);
        public Task PatchAsync(long id, UpdateCustomerDto customerDto, CancellationToken cancellationToken);
        public Task DeleteAsync(long id, CancellationToken cancellationToken);
        public Task CustomerExistsAsync(long id, CancellationToken cancellationToken);
    }
}
