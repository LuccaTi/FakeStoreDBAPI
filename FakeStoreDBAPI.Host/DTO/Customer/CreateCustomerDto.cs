using System.ComponentModel.DataAnnotations;

namespace FakeStoreDBAPI.Host.DTO.Customer
{
    public class CreateCustomerDto
    {
        [Required]
        public long AddressId { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Phone { get; set; }
    }
}
