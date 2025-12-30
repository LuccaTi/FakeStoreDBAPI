using FakeStoreDBAPI.Host.DTO.Order;
using FakeStoreDBAPI.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FakeStoreDBAPI.Host.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllAsync(cancellationToken);
            return Ok(orders);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetByIdAsync(id, cancellationToken);
            return Ok(order);
        }

        [HttpGet("day-before")]
        public async Task<IActionResult> GetAllDayBeforeAsync(CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllDayBeforeAsync(cancellationToken);
            return Ok(orders);
        }

        [HttpGet("active-or-not")]
        public async Task<IActionResult> GetAllActiveOrNotAsync(CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllActiveOrNotAsync(cancellationToken);
            return Ok(orders);
        }

        [HttpGet("{orderGuid}", Name = "GetOrderByGuid")]
        public async Task<IActionResult> GetByGuidAsync([FromRoute] string orderGuid, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetByGuidAsync(orderGuid, cancellationToken);
            return Ok(order);
        }

        [HttpGet("{orderGuid}/with-customer")]
        public async Task<IActionResult> GetByGuidWithCustomerAsync([FromRoute] string orderGuid, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetByGuidWithCustomerAsync(orderGuid, cancellationToken);
            return Ok(order);
        }

        [HttpGet("{orderGuid}/with-order-items")]
        public async Task<IActionResult> GetByGuidWithOrderItensAsync([FromRoute] string orderGuid, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetByGuidWithOrderItemsAsync(orderGuid, cancellationToken);
            return Ok(order);
        }

        [HttpGet("{id:long}/with-order-items")]
        public async Task<IActionResult> GetByIdWithOrderItemsAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetByIdWithOrderItemsAsync(id, cancellationToken);
            return Ok(order);
        }

        [HttpHead("{orderGuid}/order-exists")]
        public async Task<IActionResult> ExistsAsync([FromRoute] string orderGuid, CancellationToken cancellationToken)
        {
            await _orderService.OrderExistsAsync(orderGuid, cancellationToken);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateOrderDto orderDto, CancellationToken cancellationToken)
        {
            var createdOrder = await _orderService.PostAsync(orderDto, cancellationToken);
            return CreatedAtRoute("GetOrderByGuid", new { orderGuid = createdOrder.OrderGuid }, createdOrder);
        }

        [HttpPatch("{orderGuid}")]
        public async Task<IActionResult> PatchAsync([FromRoute] string orderGuid, [FromBody] UpdateOrderDto orderDto, CancellationToken cancellationToken)
        {
            await _orderService.PatchAsync(orderGuid, orderDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{orderGuid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string orderGuid, CancellationToken cancellationToken)
        {
            await _orderService.DeleteAsync(orderGuid, cancellationToken);
            return NoContent();
        }
    }
}
