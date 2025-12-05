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
        public async Task<IActionResult> GetAllAsync()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{orderGuid}", Name = "GetOrderByGuid")]
        public async Task<IActionResult> GetByGuidAsync([FromRoute] string orderGuid)
        {
            var order = await _orderService.GetByGuidAsync(orderGuid);
            return Ok(order);
        }

        [HttpGet("{orderGuid}/with-customer")]
        public async Task<IActionResult> GetByGuidWithCustomerAsync([FromRoute] string orderGuid)
        {
            var order = await _orderService.GetByGuidWithCustomerAsync(orderGuid);
            return Ok(order);
        }

        [HttpGet("{orderGuid}/with-order-itens")]
        public async Task<IActionResult> GetByGuidWithOrderItensAsync([FromRoute] string orderGuid)
        {
            var order = await _orderService.GetByGuidWithOrderItemsAsync(orderGuid);
            return Ok(order);
        }

        [HttpHead("{orderGuid}/order-exists")]
        public async Task<IActionResult> ExistsAsync([FromRoute] string orderGuid)
        {
            await _orderService.OrderExistsAsync(orderGuid);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateOrderDto orderDto)
        {
            var createdOrder = await _orderService.PostAsync(orderDto);
            return CreatedAtRoute("GetOrderByGuid", new { orderGuid = createdOrder.OrderGuid }, createdOrder);
        }

        [HttpPatch("{orderGuid}")]
        public async Task<IActionResult> PatchAsync([FromRoute] string orderGuid, [FromBody] UpdateOrderDto orderDto)
        {
            await _orderService.PatchAsync(orderGuid, orderDto);
            return NoContent();
        }

        [HttpDelete("{orderGuid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string orderGuid)
        {
            await _orderService.DeleteAsync(orderGuid);
            return NoContent();
        }
    }
}
