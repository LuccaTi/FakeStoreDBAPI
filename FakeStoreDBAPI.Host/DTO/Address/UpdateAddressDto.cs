namespace FakeStoreDBAPI.Host.DTO.Address
{
    public class UpdateAddressDto
    {
        public string? City { get; set; }
        public string? Street { get; set; }
        public int Number { get; set; }
        public string? Zipcode { get; set; }
    }
}
