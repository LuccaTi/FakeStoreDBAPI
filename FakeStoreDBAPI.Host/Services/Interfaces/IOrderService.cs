using FakeStoreDBAPI.Host.DTO.Order;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDto>> GetAllAsync();
        public Task<OrderDto?> GetByGuidAsync(string orderGuid);
        public Task<OrderWithCustomerDto?> GetByGuidWithCustomerAsync(string orderGuid);
        public Task<OrderDto> PostAsync(CreateOrderDto orderDto);
        public Task PatchAsync(string orderGuid, UpdateOrderDto orderDto);
        public Task DeleteAsync(string orderGuid);
        public Task OrderExistsAsync(string orderGuid);
    }
}
