using FakeStoreDBAPI.Host.DTO.Order;

namespace FakeStoreDBAPI.Host.DTO.Customer
{
    public class CustomerWithOrdersDto
    {
        public long Id { get; set; }
        public long AddressId { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreate { get; set; }

        public IEnumerable<OrderDto?> CustomerOrders { get; set; }

        public CustomerWithOrdersDto()
        {
            CustomerOrders = new List<OrderDto>();
        }
    }
}
