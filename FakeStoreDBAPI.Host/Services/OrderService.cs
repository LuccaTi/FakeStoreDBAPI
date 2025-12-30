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


        public async Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetAllAsync - Attempting to obtain all order records");
            var orders = await _context.Orders.Where(o => o.IsActive).ToListAsync(cancellationToken);
            if (orders.Count != 0)
            {
                _logger.LogDebug($"{_className} - GetAllAsync - Records obtained: {orders.Count}");
            }
            else
            {
                _logger.LogWarning($"{_className} - GetAllAsync - List of records is empty");
            }

            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetByIdAsync - Attempting to find order with ID: {id}");
            if (id == 0)
                throw new InvalidIdException($"Order ID cannot be zero!");

            var order = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
            if (order == null)
                throw new NotFoundException($"Order with ID: {id} was not found!");

            _logger.LogDebug($"{_className} - GetByIdAsync - Found order with ID: {id}");
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetAllDayBeforeAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetAllDayBeforeAsync - Attempting to obtain all order records from the previous calendar day");
            var startOfToday = DateTime.UtcNow.Date;
            var startOfYesterday = startOfToday.AddDays(-1);

            _logger.LogDebug($"{_className} - GetAllDayBeforeAsync - Filtering orders between {startOfYesterday:yyyy-MM-dd HH:mm:ss} and {startOfToday:yyyy-MM-dd HH:mm:ss}");

            var orders = await _context.Orders
                .Where(o => o.OrderDate >= startOfYesterday &&
                            o.OrderDate < startOfToday)
                .ToListAsync(cancellationToken);

            if (orders.Count != 0)
            {
                _logger.LogDebug($"{_className} - GetAllDayBeforeAsync - Records obtained: {orders.Count}");
            }
            else
            {
                _logger.LogWarning($"{_className} - GetAllDayBeforeAsync - List of records is empty");
            }

            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetAllActiveOrNotAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetAllActiveOrNotAsync - Attempting to obtain all order records including the inactives");
            var orders = await _context.Orders.ToListAsync(cancellationToken);
            if (orders.Count != 0)
            {
                _logger.LogDebug($"{_className} - GetAllActiveOrNotAsync - Records obtained: {orders.Count}");
            }
            else
            {
                _logger.LogWarning($"{_className} - GetAllActiveOrNotAsync - List of records is empty");
            }

            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto?> GetByGuidAsync(string orderGuid, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetByGuidAsync - Attempting to find order with GUID: '{orderGuid}'");

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderGuid == orderGuid, cancellationToken);
            if (order == null)
                throw new NotFoundException($"Order with GUID: '{orderGuid}' was not found");

            _logger.LogDebug($"{_className} - GetByGuidAsync - Found order with GUID: '{orderGuid}'");
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderWithCustomerDto?> GetByGuidWithCustomerAsync(string orderGuid, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetByGuidWithCustomerAsync - Attempting to find order with GUID: '{orderGuid}' and return it with customer info");

            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderGuid == orderGuid, cancellationToken);
            if (order == null || !order.IsActive)
                throw new NotFoundException($"Order with GUID: '{orderGuid}' was not found");

            _logger.LogDebug($"{_className} - GetByGuidWithCustomerAsync - Found order with GUID: '{orderGuid}'");
            return _mapper.Map<OrderWithCustomerDto>(order);
        }

        public async Task<OrderWithOrderItemsDto?> GetByGuidWithOrderItemsAsync(string orderGuid, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetByGuidWithOrderItemsAsync - Attempting to find order with GUID: '{orderGuid}' and return it with it's itens");
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.OrderGuid == orderGuid, cancellationToken);

            if (order == null || !order.IsActive)
                throw new NotFoundException($"Order with GUID: '{orderGuid}' was not found");

            _logger.LogDebug($"{_className} - GetByGuidWithOrderItemsAsync - Found order with GUID: '{orderGuid}'");
            return _mapper.Map<OrderWithOrderItemsDto>(order);
        }

        public async Task<OrderWithOrderItemsDto?> GetByIdWithOrderItemsAsync(long id, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - GetByIdWithOrderItemsAsync - Attempting to obtain order with ID: {id} and it's items");
            if (id == 0)
                throw new InvalidIdException("Order ID cannot be zero!");

            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

            if (order == null)
                throw new NotFoundException($"Order with ID: {id} was not found");

            _logger.LogDebug($"{_className} - GetByIdWithOrderItemsAsync - Found order with ID: {id}");
            return _mapper.Map<OrderWithOrderItemsDto>(order);
        }

        public async Task<OrderDto> PostAsync(CreateOrderDto orderDto, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - PostAsync - Attempting to post order");
            var order = _mapper.Map<Order>(orderDto);

            await _customerService.CustomerExistsAsync(orderDto.CustomerId, cancellationToken);

            var orderExists = false;
            orderExists = await _context.Orders.AnyAsync(o => o.OrderGuid == orderDto.OrderGuid && o.IsActive, cancellationToken);
            if (orderExists)
                throw new ConflictException($"Order with GUID: '{orderDto.OrderGuid}' already exists");

            if (orderDto.OrderItems == null || orderDto.OrderItems.Count == 0)
                throw new InvalidResourceException($"Order list of products is empty!");

            await ValidateOrderItens(orderDto.OrderItems, order.OrderGuid!, cancellationToken);
            foreach (var item in orderDto.OrderItems)
            {
                var orderProduct = _mapper.Map<OrderProduct>(item);
                order.OrderProducts.Add(orderProduct);
            }

            await ValidatePrices(order, cancellationToken);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            var postedOrder = _mapper.Map<OrderDto>(order);
            _logger.LogDebug($"{_className} - PostAsync - Posted order with GUID: '{postedOrder.OrderGuid}'");
            return postedOrder;
        }

        public async Task PatchAsync(string orderGuid, UpdateOrderDto orderDto, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - PatchAsync - Attempting to patch order with GUID: '{orderGuid}'");
            var orderToUpdate = await _context.Orders
                .Include(o => o.OrderProducts.Where(op => op.IsActive))
                .FirstOrDefaultAsync(o => o.OrderGuid == orderGuid, cancellationToken);

            if (orderToUpdate == null || !orderToUpdate.IsActive)
                throw new NotFoundException($"Order with GUID '{orderGuid}' was not found");

            var originalTotalPrice = orderToUpdate.TotalPrice;

            _mapper.Map(orderDto, orderToUpdate);

            if (orderToUpdate.TotalPrice != originalTotalPrice && (orderDto.OrderItems == null || !orderDto.OrderItems.Any()))
                throw new InvalidResourceException("Order's total price cannot be modified without providing the list of order items for validation!");

            if (orderDto.CustomerId.HasValue)
            {
                _logger.LogDebug($"{_className} - PatchAsync - Checking if customer with ID: {orderDto.CustomerId.Value} is valid");
                if (orderDto.CustomerId.Value == 0)
                    throw new InvalidIdException("Customer ID cannot be zero");

                await _customerService.CustomerExistsAsync(orderDto.CustomerId.Value, cancellationToken);

                orderToUpdate.CustomerId = orderDto.CustomerId.Value;
                _logger.LogDebug($"{_className} - PatchAsync - Order GUID: '{orderGuid}' patched it's customer ID to: {orderDto.CustomerId.Value}");
            }
            else
            {
                _context.Entry(orderToUpdate).Property(x => x.CustomerId).IsModified = false;
            }

            if (orderDto.OrderItems != null && orderDto.OrderItems.Count != 0)
            {
                await ValidateOrderItens(orderDto.OrderItems, orderGuid, cancellationToken);

                var existingItems = orderToUpdate.OrderProducts!.ToDictionary(op => op.ProductId);
                var dtoItems = orderDto.OrderItems.ToDictionary(item => item.ProductId);

                foreach (var dtoItem in orderDto.OrderItems)
                {
                    if (existingItems.TryGetValue(dtoItem.ProductId, out var existingItem))
                    {
                        _mapper.Map(dtoItem, existingItem);
                    }
                    else
                    {
                        var newOrderItem = new OrderProduct()
                        {
                            ProductId = dtoItem.ProductId,
                            Quantity = dtoItem.Quantity,
                            TotalPrice = dtoItem.TotalPrice
                        };
                        orderToUpdate.OrderProducts!.Add(newOrderItem);
                    }
                }

                foreach (var existingItem in existingItems.Values)
                {
                    if (!dtoItems.ContainsKey(existingItem.ProductId))
                        existingItem.IsActive = false;
                }
            }

            await ValidatePrices(orderToUpdate, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug($"{_className} - PatchAsync - Patched order with GUID: '{orderGuid}'");
        }

        public async Task DeleteAsync(string orderGuid, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - DeleteAsync - Attempting to deactive order with GUID: '{orderGuid}' and it's dependencies (ORDER_PRODUCT)");
            var orderToDelete = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.OrderGuid == orderGuid, cancellationToken);

            if (orderToDelete == null || !orderToDelete.IsActive)
                throw new NotFoundException($"Order GUID: {orderGuid} not found");

            if (orderToDelete.OrderProducts != null && orderToDelete.OrderProducts.Any())
            {
                foreach (var dependency in orderToDelete.OrderProducts)
                {
                    dependency.IsActive = false;
                }
                _logger.LogDebug($"{_className} - DeleteAsync - Associated dependecies deactivated: {orderToDelete.OrderProducts.Count}");
            }
            else
            {
                _logger.LogDebug($"{_className} - DeleteAsync - order didn't have any dependencies associated with it");
            }

            orderToDelete.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug($"{_className} - DeleteAsync - Successfully deactivated order with GUID: '{orderGuid}'");
        }

        public async Task OrderExistsAsync(string orderGuid, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - OrderExistsAsync - Checking if order with GUID: '{orderGuid}' exists and is active");
            bool orderExists = false;
            orderExists = await _context.Orders.AnyAsync(o => o.OrderGuid == orderGuid && o.IsActive, cancellationToken);
            if (!orderExists)
                throw new NotFoundException($"Order with GUID: '{orderGuid}' does not exists or is inactive");

            _logger.LogDebug($"{_className} - OrderExistsAsync - Order exists and is active");
        }

        public async Task ValidatePrices(Order order, CancellationToken cancellationToken)
        {
            if (order.OrderProducts != null && order.OrderProducts.Count != 0)
            {
                _logger.LogDebug($"{_className} - ValidatePrices - Validating if order with GUID: {order.OrderGuid} total price and itens prices are valid");

                var productIds = order.OrderProducts.Select(op => op.ProductId).ToList();

                var productsFromDb = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id, cancellationToken);

                decimal calculatedTotalPrice = 0.0M;
                foreach (var item in order.OrderProducts)
                {
                    if (!item.IsActive)
                        continue;

                    if (!productsFromDb.TryGetValue(item.ProductId, out var product))
                        throw new NotFoundException($"Product with ID: {item.ProductId} not found during price validation!");

                    decimal dtoProductPrice = item.TotalPrice / item.Quantity;

                    if (dtoProductPrice != product!.Price)
                    {
                        _logger.LogWarning($"{_className} - ValidatePrices - Product price obtained in HTTP request is different from product price in database!");
                        throw new InvalidResourceException("Product price obtained in HTTP request is invalid!");
                    }

                    calculatedTotalPrice += item.TotalPrice;
                }

                if (calculatedTotalPrice != order.TotalPrice)
                {
                    _logger.LogWarning($"{_className} - ValidatePrices - Order total price obtained in HTTP request is different from order total price in database!");
                    throw new InvalidResourceException("Order total price obtained in HTTP request is invalid!");
                }
            }
        }

        public async Task ValidateOrderItens(IEnumerable<CreateOrderItemDto> orderItens, string orderGuid, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{_className} - ValidateOrderItens - Validating items from order with GUID: {orderGuid}");
            var productIds = orderItens.Select(p => p.ProductId);
            var productsFromDb = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, cancellationToken);

            foreach (var item in orderItens)
            {
                if (item.ProductId == 0)
                    throw new InvalidIdException("Product ID cannot be zero!");

                if (item.Quantity == 0)
                    throw new InvalidResourceException("Product quantity cannot be zero!");

                if (item.TotalPrice == 0)
                    throw new InvalidResourceException("Product price cannot be zero!");

                if (!productsFromDb.ContainsKey(item.ProductId))
                    throw new NotFoundException($"Product with ID: {item.ProductId} was not found!");
            }
        }
    }
}
