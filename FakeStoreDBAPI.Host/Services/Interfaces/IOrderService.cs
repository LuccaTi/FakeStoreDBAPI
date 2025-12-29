using FakeStoreDBAPI.Host.DTO.Order;

namespace FakeStoreDBAPI.Host.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<OrderDto>> GetAllDayBeforeAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<OrderDto>> GetAllActiveOrNotAsync(CancellationToken cancellationToken);
        public Task<OrderDto?> GetByGuidAsync(string orderGuid, CancellationToken cancellationToken);
        public Task<OrderWithCustomerDto?> GetByGuidWithCustomerAsync(string orderGuid, CancellationToken cancellationToken);
        public Task<OrderWithOrderItemsDto?> GetByGuidWithOrderItemsAsync(string orderGuid, CancellationToken cancellationToken);
        public Task<OrderDto> PostAsync(CreateOrderDto orderDto, CancellationToken cancellationToken);
        public Task PatchAsync(string orderGuid, UpdateOrderDto orderDto, CancellationToken cancellationToken);
        public Task DeleteAsync(string orderGuid, CancellationToken cancellationToken);
        public Task OrderExistsAsync(string orderGuid, CancellationToken cancellationToken);
    }
}
