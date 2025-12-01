using FakeStoreDBAPI.Host.DTO.Address;

namespace FakeStoreDBAPI.Host.DTO.Customer
{
    public class CustomerWithAddressDto
    {
        public long Id { get; set; }
        public long AddressId { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }

        public AddressDto? Address { get; set; }
    }
}
