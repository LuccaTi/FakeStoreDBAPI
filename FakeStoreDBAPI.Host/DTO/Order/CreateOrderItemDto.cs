using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.DTO.Order
{
    public class CreateOrderItemDto
    {
        [Required]
        public long ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }

    }
}
