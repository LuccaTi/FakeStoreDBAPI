
namespace FakeStoreDBAPI.Host.DTO.Order
{
    public class OrderItemDto
    {
        
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
