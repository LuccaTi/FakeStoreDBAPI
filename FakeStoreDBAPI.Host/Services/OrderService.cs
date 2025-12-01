using AutoMapper;
using FakeStoreDBAPI.Host.Data;
using FakeStoreDBAPI.Host.DTO.Order;
using FakeStoreDBAPI.Host.Exceptions;
using FakeStoreDBAPI.Host.Models;
using FakeStoreDBAPI.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FakeStoreDBAPI.Host.Services
{
    public class OrderService : IOrderService
    {
        private const string _className = "OrderService";
        private readonly FakeStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly ICustomerService _customerService;

        public OrderService(FakeStoreDbContext context, IMapper mapper, ILogger<OrderService> logger, ICustomerService customerService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _customerService = customerService;
        }


        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            _logger.LogDebug($"{_className} - Attempting to obtain all order records");
            var orders = await _context.Orders.Where(o => o.IsActive).ToListAsync();
            if (orders.Count != 0)
            {
                _logger.LogDebug($"{_className} - Records obtained: {orders.Count}");
            }
            else
            {
                _logger.LogWarning($"{_className} - List of records is empty");
            }

            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto?> GetByGuidAsync(string orderGuid)
        {
            _logger.LogDebug($"{_className} - Attempting to find order GUID: '{orderGuid}'");

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderGuid == orderGuid);
            if (order == null || !order.IsActive)
                throw new NotFoundException($"Order GUID: '{orderGuid}' was not found");

            _logger.LogDebug($"{_className} - Found order GUID: '{orderGuid}'");
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderWithCustomerDto?> GetByGuidWithCustomerAsync(string orderGuid)
        {
            _logger.LogDebug($"{_className} - Attempting to find order GUID: '{orderGuid}' and return it with customer info");

            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderGuid == orderGuid);
            if (order == null || !order.IsActive)
                throw new NotFoundException($"Order GUID: '{orderGuid}' was not found");

            _logger.LogDebug($"{_className} - Found order GUID: '{orderGuid}'");
            return _mapper.Map<OrderWithCustomerDto>(order);
        }

        public async Task<OrderDto> PostAsync(CreateOrderDto orderDto)
        {
            _logger.LogDebug($"{_className} - Attempting to post order");
            var order = _mapper.Map<Order>(orderDto);

            await _customerService.CustomerExistsAsync(orderDto.CustomerId);

            var orderExists = false;
            orderExists = await _context.Orders.AnyAsync(o => o.OrderGuid == orderDto.OrderGuid);
            if (orderExists)
                throw new ConflictException($"Order GUID: '{orderDto.OrderGuid}' already exists");

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var postedOrder = _mapper.Map<OrderDto>(order);
            _logger.LogDebug($"{_className} - Posted order GUID: '{postedOrder.OrderGuid}'");
            return postedOrder;
        }

        public async Task PatchAsync(string orderGuid, UpdateOrderDto orderDto)
        {
            _logger.LogDebug($"{_className} - Attempting to patch order GUID: '{orderGuid}'");
            var orderToUpdate = await _context.Orders.FirstOrDefaultAsync(o => o.OrderGuid == orderGuid);
            if (orderToUpdate == null || !orderToUpdate.IsActive)
                throw new NotFoundException($"Order GUID '{orderGuid}' was not found");

            // Map first because of possible zero value to property orderDto.CustomerId
            _mapper.Map(orderDto, orderToUpdate);

            if (orderDto.CustomerId.HasValue)
            {
                _logger.LogDebug($"{_className} - Checking if customer ID: {orderDto.CustomerId.Value} is valid");
                if (orderDto.CustomerId.Value == 0)
                    throw new InvalidIdException("Customer ID cannot be zero");

                await _customerService.CustomerExistsAsync(orderDto.CustomerId.Value);

                orderToUpdate.CustomerId = orderDto.CustomerId.Value;
                _logger.LogDebug($"Order GUID: '{orderGuid}' customer ID patched to: {orderDto.CustomerId.Value}");
            }
            else
            {
                _context.Entry(orderToUpdate).Property(x => x.CustomerId).IsModified = false;
            }

            await _context.SaveChangesAsync();
            _logger.LogDebug($"{_className} - Patched order GUID: '{orderGuid}'");
        }

        public async Task DeleteAsync(string orderGuid)
        {
            _logger.LogDebug($"{_className} - Attempting to deactive order GUID: '{orderGuid}'");
            var orderToDelete = await _context.Orders.FirstOrDefaultAsync(o => o.OrderGuid == orderGuid);
            if (orderToDelete == null || !orderToDelete.IsActive)
                throw new NotFoundException($"Order GUID: {orderGuid} not found");

            orderToDelete.IsActive = false;
            await _context.SaveChangesAsync();
            _logger.LogDebug($"{_className} - Successfully deactivated order GUID: '{orderGuid}'");
        }

        public async Task OrderExistsAsync(string orderGuid)
        {
            _logger.LogDebug($"{_className} - Checking if order GUID: '{orderGuid}' exists and is active");
            bool orderExists = false;
            orderExists = await _context.Orders.AnyAsync(o => o.OrderGuid == orderGuid && o.IsActive);
            if (!orderExists)
                throw new NotFoundException($"Order GUID: '{orderGuid}' does not exists or is inactive");

            _logger.LogDebug($"{_className} - Order exists and is active");
        }
    }
}
