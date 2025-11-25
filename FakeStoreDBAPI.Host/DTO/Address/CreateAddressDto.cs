using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.DTO.Address
{
    public class CreateAddressDto
    {
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Street { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string? Zipcode { get; set; }
    }
}
