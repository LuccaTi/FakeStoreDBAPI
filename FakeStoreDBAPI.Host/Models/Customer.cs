using FakeStoreDBAPI.Host.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeStoreDBAPI.Host.Models
{
    public class Customer : IAuditable
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(Address))]
        public long AddressId { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string? UserName { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Password { get; set; }
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }

        public Address? Address { get; set; }
    }
}
