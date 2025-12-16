using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.DTO.Address
{
    public class StreetNumberDto
    {
        [Required]
        public string? Street { get; set; }
        [Required]
        public int Number { get; set; }
    }
}
