using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.DTO.Customer
{
    public class LoginRequestDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
