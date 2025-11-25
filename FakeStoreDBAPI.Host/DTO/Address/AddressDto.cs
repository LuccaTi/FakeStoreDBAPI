namespace FakeStoreDBAPI.Host.DTO.Address
{
    public class AddressDto
    {
        public long Id { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int Number { get; set; }
        public string? Zipcode { get; set; }
        public bool IsActive { get; set; }
    }
}
