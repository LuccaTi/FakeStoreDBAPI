namespace FakeStoreDBAPI.Host.DTO.Order
{
    public class OrderWithOrderItemsDto
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string? OrderGuid { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime DeliveredDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? ShippingStatus { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<OrderItemDto> OrderItems { get; set; }

        public OrderWithOrderItemsDto()
        {
            OrderItems = new List<OrderItemDto>();
        }

    }
}
